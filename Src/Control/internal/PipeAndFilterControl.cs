// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PipeFilterCore
{
    internal class PipeAndFilterControl<T> : IDisposable,IPipeAndFilterInit<T> where T : class
    {
        private readonly IPipeAndFilterOptions<T> _parameters;
        private readonly List<Task> _tasks = new();
        private readonly object _lockObj = new();
        private readonly List<string> _sequencePipes = new();
        private readonly Dictionary<string, string?> _savedvalues = new();
        private readonly List<(string id, string? value, bool toremove)> _savedtaskvalues = new();
        private readonly Dictionary<string, List<PipeStatus>> _status = new();

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

        public IPipeAndFilterInit<T> CorrelationId(string? value)
        {
            _cid = value;
            return this;
        }

        public IPipeAndFilterInit<T> Init(T contract)
        {
            _contract = contract;
            return this;
        }

        public IPipeAndFilterInit<T> Logger(ILogger? value)
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
            _sequencePipes.AddRange(_parameters.Pipes.Select(x => x.Id));
            foreach (var item in _sequencePipes)
            {
                _status.Add(item, new());
            }
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
                    var sta = HandlerStatus.Created;
                    try
                    {
                        if (_parameters.AggregateTasks[_currentPipe!])
                        {
                            _savedtaskvalues.Clear();
                            await ExecuteTasksPipes(_parameters.Tasks[_currentPipe!]);
                            if (!IsEnd)
                            { 
                                foreach(var (id, value, toremove) in _savedtaskvalues) 
                                {
                                    if (toremove)
                                    {
                                        _savedvalues.Remove(id);
                                    }
                                    else
                                    {
                                        if (_savedvalues.ContainsKey(id))
                                        {
                                            _savedvalues[id] = value;
                                        }
                                        else
                                        {
                                            _savedvalues.Add(id,value);
                                        }
                                    }
                                }
                            }
                            _savedtaskvalues.Clear();
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
                                _savedvalues.ToImmutableDictionary(),
                                _prevPipe,
                                _currentPipe!,
                                aliasprev, aliascur);
                            await _parameters.Pipes.First(x => x.Id == _currentPipe!).Handler(evt, _pipects!.Token);
                            sta = HandlerStatus.Completed;
                            elapsed = tm.Elapsed;
                            EnsureResultEventPipe(evt);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        elapsed = tm.Elapsed;
                        sta = HandlerStatus.Canceled;
                        _finished = true;
                    }
                    catch (Exception ex)
                    {
                        elapsed = tm.Elapsed;
                        sta = HandlerStatus.Faulted;
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
                    _status[_currentPipe].Add(new PipeStatus(
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
                        x.Id,
                        _parameters.IdToAlias[x.Id],
                        _status[x.Id].ToImmutableArray())).ToImmutableArray()
                );
        }

        private async Task NextPipe()
        {
            while (!IsEnd)
            {
                var isok = true;
                foreach (var itemcond in _parameters.PreConditions[_currentPipe!])
                {
                    var condpipeType = string.IsNullOrEmpty(itemcond.GotoId) ? HandlerType.Condition : HandlerType.ConditionGoto;
                    var sta = HandlerStatus.Created;
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
                        _savedvalues.ToImmutableDictionary(),
                        _prevPipe,
                        _currentPipe!,
                        aliasprev, aliascur);

                    var elapsed = TimeSpan.Zero;
                    var tm = Stopwatch.StartNew();
                    try
                    {
                        isok = await itemcond.Handle!(evt, _pipects!.Token);
                        elapsed = tm.Elapsed;
                        sta = HandlerStatus.Completed;
                        EnsureResultEventPipe(evt);
                    }
                    catch (OperationCanceledException)
                    {
                        elapsed = tm.Elapsed;
                        sta = HandlerStatus.Canceled;
                        _finished = true;
                    }
                    catch (Exception ex)
                    {
                        elapsed = tm.Elapsed;
                        sta = HandlerStatus.Faulted;
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
                    _status[_currentPipe!].Add(new PipeStatus(
                        condpipeType,
                        sta,
                        elapsed,
                        itemcond.Name,
                        itemcond.GotoId, isok));
                    if (!IsEnd && ((!isok && condpipeType == HandlerType.Condition) || (isok && condpipeType == HandlerType.ConditionGoto)))
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
                    if (!IsEnd && !isok && condpipeType == HandlerType.ConditionGoto)
                    { 
                        isok = true;
                    }                
                }
                if (isok)
                {
                    break;
                }
            }
        }

        private void EnsureResultEventPipe(EventPipe<T> eventPipe)
        {
            if (eventPipe.FinishedPipeAndFilter)
            {
                _finished = true;
            }
            foreach (var (id, value, toremove) in eventPipe.ToSaveRemove)
            {
                if (toremove)
                {
                    _savedvalues.Remove(id);
                }
                else
                {
                    if (_savedvalues.ContainsKey(id))
                    {
                        _savedvalues[id] = value;
                    }
                    else
                    {
                        _savedvalues.Add(id, value);
                    }
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

        private async Task ExecuteTasksPipes(IImmutableList<PipeTask<T>> tasks)
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
                    if (tasks[i].Condition.HasValue)
                    {
                        HandlerStatus sta;
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
                            _savedvalues.ToImmutableDictionary(),
                            _prevPipe,
                            _currentPipe!,
                            aliasprev, aliascur);

                        var elapsed = TimeSpan.Zero;
                        var tm = Stopwatch.StartNew();
                        try
                        {
                            isvalidtask = await tasks[i].Condition!.Value.Handle!(evt, _pipects!.Token);
                            elapsed = tm.Elapsed;
                            sta = HandlerStatus.Completed;
                            EnsureResultEventPipe(evt);
                        }
                        catch (OperationCanceledException)
                        {
                            elapsed = tm.Elapsed;
                            sta = HandlerStatus.Canceled;
                            _finished = true;
                        }
                        catch (Exception ex)
                        {
                            elapsed = tm.Elapsed;
                            sta = HandlerStatus.Faulted;
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
                        _status[_currentPipe!].Add(new PipeStatus(
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
                                handle = tasks[index].Handler;
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
                                    _savedvalues.ToImmutableDictionary(),
                                    _prevPipe,
                                    _currentPipe!,
                                    aliasprev, aliascur);
                            }
                            var elapsed = TimeSpan.Zero;
                            var sta = HandlerStatus.Created;
                            var tm = Stopwatch.StartNew();
                            try
                            {
                                handle(evt, _pipects!.Token).Wait(_pipects.Token);
                                elapsed = tm.Elapsed;
                                sta = HandlerStatus.Completed;
                                EnsureResultEventPipeTask(evt);
                            }
                            catch (OperationCanceledException)
                            {
                                elapsed = tm.Elapsed;
                                sta = HandlerStatus.Canceled;
                                lock (_lockObj)
                                {
                                    _finished = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                elapsed = tm.Elapsed;
                                sta = HandlerStatus.Faulted;
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
                                _status[_currentPipe!].Add(new PipeStatus(
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

        private void EnsureResultEventPipeTask(EventPipe<T> eventPipe)
        {
            lock (_lockObj)
            {
                if (eventPipe.FinishedPipeAndFilter)
                {
                    _finished = true;
                }
                _savedtaskvalues.AddRange(eventPipe.ToSaveRemove);
            }
        }
    }
}
