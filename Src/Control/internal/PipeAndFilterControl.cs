// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PipeFilterCore.CommandsInterface;

namespace PipeFilterCore.Control
{
    internal class PipeAndFilterControl<T> : IDisposable,IPipeAndFilterRunService<T> where T : class
    {
        private readonly IPipeAndFilterOptions<T> _parameters;
        private readonly List<Task> _tasks = new();
        private readonly object _lockObj = new();
        private readonly List<string> _sequencePipes = new();
        private readonly List<(string? Alias, string Id, string? Value)> _savedvalues = new();
        private readonly List<(string? Alias, string Id, string? Value)> _savedtaskvalues = new();

        private bool _disposed;
        private CancellationTokenSource? _pipects;
        private ILogger _logger = NullLogger.Instance;
        private string? _cid;
        private PipeAndFilterException? _lastexception;
        private T? _contract;
        private int _currentPipeIndex;
        private string? _currentPipe;
        private string? _prevPipe;
        private bool _abort;
        private bool _finished;
        private bool IsEnd => _finished || _abort || _pipects!.IsCancellationRequested;

        public PipeAndFilterControl(IPipeAndFilterOptions<T> parameters)
        {
            _parameters = parameters;
        }

        #region IPipeAndFilterRunService

        public string? ServiceId => _parameters.ServiceId;

        public IPipeAndFilterRunService<T> CorrelationId(string? value)
        {
            _cid = value;
            return this;
        }

        public IPipeAndFilterRunService<T> Init(T contract)
        {
            _contract = contract;
            return this;
        }

        public IPipeAndFilterRunService<T> Logger(ILogger? value)
        {
            _logger = value ?? NullLogger.Instance;
            return this;
        }

        public async ValueTask<ResultPipeAndFilter<T>> Run(CancellationToken? cancellation = null)
        {

            _pipects = CancellationTokenSource.CreateLinkedTokenSource(cancellation??CancellationToken.None);

            InitControl();

            var aux = await ExecutePipes();

            Dispose();

            return aux;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var item in _tasks.Where(x => x.IsCompleted))
                    {
                        item.Dispose();
                    }
                    _tasks.Clear();
                    _pipects?.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion

        private void InitControl()
        {
            _sequencePipes.AddRange(_parameters.Pipes.Keys.ToArray());
        }

        private async ValueTask<ResultPipeAndFilter<T>> ExecutePipes()
        {
            _currentPipeIndex = 0;
            _currentPipe = _sequencePipes[0];
            while (!IsEnd)
            {
                await NextPipe();
                if (!IsEnd)
                {
                    var tm = Stopwatch.StartNew();
                    var elapsed = TimeSpan.Zero;
                    var sta = TaskStatus.WaitingForActivation;
                    try
                    {
                        _savedtaskvalues.Clear();
                        if (_parameters.Pipes[_currentPipe!].aggregateTasks)
                        {
                            await ExecuteTasksPipes(_parameters.Pipes[_currentPipe!].tasks!);
                        }
                        if (!IsEnd)
                        {
                            string? aliasprev = null;
                            if (!string.IsNullOrEmpty(_prevPipe))
                            {
                                aliasprev = _parameters.IdToAlias[_prevPipe];
                            };
                            string? aliascur = _parameters.IdToAlias[_currentPipe!];
                            var evt = new EventPipe<T>(
                                _cid,
                                _logger,
                                ChangeContract!,
                                _savedvalues.ToImmutableArray(),
                                _savedtaskvalues.ToImmutableArray(),
                                _prevPipe,
                                _currentPipe!,
                                aliasprev, aliascur);
                            await _parameters.Pipes[_currentPipe!].pipehandle(evt, _pipects!.Token);
                            sta = TaskStatus.RanToCompletion;
                            elapsed = tm.Elapsed;
                            EnsureResultEventPipe(_currentPipe!, evt);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        elapsed = tm.Elapsed;
                        sta = TaskStatus.Canceled;
                        _finished = true;
                    }
                    catch (Exception ex)
                    {
                        elapsed = tm.Elapsed;
                        sta = TaskStatus.Faulted;
                        _lastexception = new PipeAndFilterException(
                            new PipeStatus(
                                HandlerType.Pipe,
                                sta,
                                elapsed,
                                _parameters.IdToAlias[_currentPipe!],
                                null, true),
                            "Error handler Pipe",
                            ex);
                        _abort = true;
                        _finished = true;
                    }
                    tm.Stop();
                    _parameters.Pipes[_currentPipe!].status.Add(new PipeStatus(
                        HandlerType.Pipe,
                        sta,
                        elapsed,
                        _parameters.IdToAlias[_currentPipe!],
                        null, true));

                }
                if (_lastexception != null)
                {
                    _pipects!.Cancel();
                }
                if (!IsEnd)
                {
                    _currentPipeIndex++;
                    if (_currentPipeIndex < _sequencePipes.Count)
                    {
                        _prevPipe = _currentPipe;
                        _currentPipe = _sequencePipes[_currentPipeIndex];
                    }
                    else
                    {
                        _finished = true;
                    }
                }
            }
            return new ResultPipeAndFilter<T>(
                _contract,
                _abort,
                _lastexception,
                _parameters.Pipes.Select(x =>
                    new PipeRanStatus(
                        x.Key,
                        _parameters.IdToAlias[x.Key],
                        x.Value.status)).ToImmutableArray()
                );
        }

        private async Task NextPipe()
        {
            while (!IsEnd)
            {
                var isok = true;
                foreach (var itemcond in _parameters.Pipes[_currentPipe!].precondhandle)
                {
                    var condpipeType = string.IsNullOrEmpty(itemcond.GotoId) ? HandlerType.Condition : HandlerType.ConditionGoto;
                    var sta = TaskStatus.RanToCompletion;
                    string? aliasprev = null;
                    if (!string.IsNullOrEmpty(_prevPipe))
                    {
                        aliasprev = _parameters.IdToAlias[_prevPipe];
                    };
                    string? aliascur = _parameters.IdToAlias[_currentPipe!];

                    var evt = new EventPipe<T>(
                        _cid,
                        _logger,
                        ChangeContract!,
                        _savedvalues.ToImmutableArray(),
                        _savedtaskvalues.ToImmutableArray(),
                        _prevPipe,
                        _currentPipe!,
                        aliasprev, aliascur);

                    var elapsed = TimeSpan.Zero;
                    var tm = Stopwatch.StartNew();
                    try
                    {
                        isok = await itemcond.Handle!(evt, _pipects!.Token);
                        elapsed = tm.Elapsed;
                        EnsureResultEventPipe(_currentPipe!, evt);
                    }
                    catch (OperationCanceledException)
                    {
                        elapsed = tm.Elapsed;
                        sta = TaskStatus.Canceled;
                        _finished = true;
                    }
                    catch (Exception ex)
                    {
                        elapsed = tm.Elapsed;
                        sta = TaskStatus.Faulted;
                        _lastexception = new PipeAndFilterException(
                             new PipeStatus(
                                 condpipeType,
                                 sta,
                                 elapsed,
                                 itemcond.Name,
                                 itemcond.GotoId, isok),
                             "Error handler condition",
                             ex);
                        _abort = true;
                        _finished = true;
                        _pipects!.Cancel(false);
                    }
                    tm.Stop();
                    _parameters.Pipes[_currentPipe!].status.Add(new PipeStatus(
                        condpipeType,
                        sta,
                        elapsed,
                        itemcond.Name,
                        itemcond.GotoId, isok));
                    if (!IsEnd)
                    {
                        if ((!isok && condpipeType == HandlerType.Condition) || (isok && condpipeType == HandlerType.ConditionGoto))
                        {
                            if (!string.IsNullOrEmpty(itemcond.GotoId))
                            {
                                _prevPipe = _currentPipe;
                                _currentPipe = _parameters.AliasToId[itemcond.GotoId];
                                _currentPipeIndex = _sequencePipes.IndexOf(_currentPipe);
                                return;
                            }
                            _currentPipeIndex++;
                            if (_currentPipeIndex < _sequencePipes.Count)
                            {
                                _prevPipe = _currentPipe;
                                _currentPipe = _sequencePipes[_currentPipeIndex];
                            }
                            else
                            {
                                _finished = true;
                            }
                            return;
                        }
                    }
                }
                if (isok)
                {
                    break;
                }
            }
        }

        private void EnsureResultEventPipe(string idpipe, EventPipe<T> eventPipe)
        {
            if (eventPipe.FinishedPipeAndFilter)
            {
                _finished = true;
            }
            var alias = _parameters.IdToAlias[idpipe];
            if (eventPipe.ToRemove)
            {
                var index = _savedvalues.FindIndex(x => x.Id == idpipe);
                if (index >= 0)
                {
                    _savedvalues.RemoveAt(index);
                }
            }
            else if (eventPipe.IsSaved)
            {
                var index = _savedvalues.FindIndex(x => x.Id == idpipe);
                if (index >= 0)
                {
                    _savedvalues[index] = (alias, idpipe, eventPipe.ValueToSave);
                }
                else
                {
                    _savedvalues.Add((alias, idpipe, eventPipe.ValueToSave));
                }
            }
        }

        private void ChangeContract(Action<T> eventchange)
        {
            lock (_lockObj)
            {
                if (_contract == null)
                {
                    return;
                }
                eventchange(_contract);
            }
        }

        private async Task ExecuteTasksPipes(List<(string Id, Func<EventPipe<T>, CancellationToken, Task> TaskHandle, PipeCondition<T>? TaskCondition, string? NameTask, string? NameCondition)> tasks)
        {
            var i = 0;
            var maxDegreeProcess = _parameters.MaxDegreeProcess[_currentPipe!];
            _savedtaskvalues.Clear();
            var emptytask = _savedtaskvalues.ToImmutableArray();
            do
            {
                var degreecount = 0;
                do
                {
                    var isvalidtask = true;
                    if (tasks[i].TaskCondition.HasValue)
                    {
                        TaskStatus sta;
                        string? aliasprev = null;
                        if (!string.IsNullOrEmpty(_prevPipe))
                        {
                            aliasprev = _parameters.IdToAlias[_prevPipe];
                        };
                        string? aliascur = _parameters.IdToAlias[_currentPipe!];

                        var evt = new EventPipe<T>(
                            _cid,
                            _logger,
                            ChangeContract!,
                            _savedvalues.ToImmutableArray(),
                            emptytask,
                            _prevPipe,
                            _currentPipe!,
                            aliasprev, aliascur);

                        var elapsed = TimeSpan.Zero;
                        var tm = Stopwatch.StartNew();
                        try
                        {
                            isvalidtask = await tasks[i].TaskCondition!.Value.Handle!(evt, _pipects!.Token);
                            elapsed = tm.Elapsed;
                            sta = TaskStatus.RanToCompletion;
                            EnsureResultEventPipeTask(tasks[i].Id, tasks[i].NameTask, evt);
                        }
                        catch (OperationCanceledException)
                        {
                            elapsed = tm.Elapsed;
                            sta = TaskStatus.Canceled;
                            _finished = true;
                        }
                        catch (Exception ex)
                        {
                            elapsed = tm.Elapsed;
                            sta = TaskStatus.Faulted;
                            _lastexception = new PipeAndFilterException(
                                new PipeStatus(
                                    HandlerType.ConditionTask,
                                    sta,
                                    elapsed,
                                    tasks[i].NameTask,
                                    null, isvalidtask),
                                "Error handler Condition Task",
                                ex);
                            _abort = true;
                            _finished = true;
                            _pipects!.Cancel(false);
                        }
                        tm.Stop();
                        _parameters.Pipes[_currentPipe!].status.Add(new PipeStatus(
                            HandlerType.ConditionTask,
                            sta,
                            elapsed,
                            tasks[i].NameTask,
                            null, isvalidtask));
                        if (IsEnd)
                        {
                            isvalidtask = false;
                        }
                    }
                    if (isvalidtask)
                    {
                        _tasks.Add(new Task((param) =>
                        {
                            string? aliasprev = null;
                            string? aliascur = null;
                            EventPipe<T> evt;
                            Func<EventPipe<T>, CancellationToken, Task> handle;
                            string taskid;
                            string? taskname;
                            lock (_lockObj)
                            {
                                var index = (int)param!;
                                handle = tasks[index].TaskHandle;
                                taskid = tasks[index].Id;
                                taskname = tasks[index].NameTask;
                                if (!string.IsNullOrEmpty(_prevPipe))
                                {
                                    aliasprev = _parameters.IdToAlias[_prevPipe];
                                };
                                aliascur = _parameters.IdToAlias[_currentPipe!];
                                evt = new EventPipe<T>(
                                    _cid,
                                    _logger,
                                    ChangeContract!,
                                    _savedvalues.ToImmutableArray(),
                                    _savedtaskvalues.ToImmutableArray(),
                                    _prevPipe,
                                    _currentPipe!,
                                    aliasprev, aliascur);
                            }
                            var elapsed = TimeSpan.Zero;
                            var sta = TaskStatus.RanToCompletion;
                            var tm = Stopwatch.StartNew();
                            try
                            {
                                handle(evt, _pipects!.Token).Wait(_pipects.Token);
                                elapsed = tm.Elapsed;
                                EnsureResultEventPipeTask(taskid, taskname, evt);
                            }
                            catch (OperationCanceledException)
                            {
                                elapsed = tm.Elapsed;
                                sta = TaskStatus.Canceled;
                                EnsureResultEventPipeTask(taskid, taskname, evt);
                                lock (_lockObj)
                                {
                                    _finished = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                elapsed = tm.Elapsed;
                                sta = TaskStatus.Faulted;
                                EnsureResultEventPipeTask(taskid, taskname, evt);
                                lock (_lockObj)
                                {
                                    _lastexception = new PipeAndFilterException(
                                        new PipeStatus(
                                            HandlerType.ConditionTask,
                                            sta,
                                            elapsed,
                                            taskname,
                                            null, isvalidtask),
                                        "Error handler Task",
                                        ex);
                                    _abort = true;
                                    _finished = true;
                                }
                            }
                            tm.Stop();
                            lock (_lockObj)
                            {
                                _parameters.Pipes[_currentPipe!].status.Add(new PipeStatus(
                                    HandlerType.Task,
                                    sta,
                                    elapsed,
                                    taskname,
                                    null, isvalidtask));
                                if (_lastexception != null)
                                {
                                    _pipects!.Cancel();
                                }
                            }
                        }, i, _pipects!.Token));
                        degreecount++;
                    }
                    i++;
                } while (!IsEnd && i < tasks.Count && degreecount < maxDegreeProcess);
                if (!IsEnd)
                {
                    try
                    {
                        var taskwait = _tasks
                            .Where(x => x.Status == TaskStatus.Created)
                            .ToArray();
                        for (int pos = 0; pos < taskwait.Length; pos++)
                        {
                            taskwait[pos].Start();
                        }
                        Task.WaitAll(taskwait, _pipects!.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        //none
                    }
                }
            } while (!IsEnd && i < tasks.Count);
        }

        private void EnsureResultEventPipeTask(string idtask, string? name, EventPipe<T> eventPipe)
        {
            lock (_lockObj)
            {
                if (eventPipe.FinishedPipeAndFilter)
                {
                    _finished = true;
                }
                if (eventPipe.ToRemove)
                {
                    var index = _savedtaskvalues.FindIndex(x => x.Id == idtask);
                    if (index >= 0)
                    {
                        _savedtaskvalues.RemoveAt(index);
                    }
                }
                else if (eventPipe.IsSaved)
                {
                    var index = _savedvalues.FindIndex(x => x.Id == idtask);
                    if (index >= 0)
                    {
                        _savedtaskvalues[index] = (name, idtask, eventPipe.ValueToSave);
                    }
                    else
                    {
                        _savedtaskvalues.Add((name, idtask, eventPipe.ValueToSave));
                    }
                }
            }
        }


    }
}
