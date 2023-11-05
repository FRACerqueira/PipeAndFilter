// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace PipeFilterPlus
{
    internal class PipeAndFilterControl<T> : IDisposable, IPipeAndFilterInit<T>, IPipeAndFilter<T>, IPipeAndFilterConditions<T>, IPipeAndFilterTasks<T> where T : class
    {
        private readonly List<Task> _tasks = new();
        private readonly object _lockObj = new();
        private readonly List<(string? Alias, string Id, string? Value)> _savedvalues = new();
        private readonly List<(string? Alias, string Id, string? Value)> _savedtaskvalues = new();
        private readonly CancellationToken _ctstoken;
        private readonly Dictionary<string, string> _aliasToId = new();
        private readonly Dictionary<string, string?> _idToAlias = new();
        private readonly List<string> _sequencePipes = new();

        private ILogger _logger = NullLogger.Instance;
        private string? _cid;

        private bool _disposed;
        private PipeAndFilterException? _lastexception;

        private int _maxDegreeProcess = Environment.ProcessorCount;
        private T? _contract;

        private string? _currentPipe;
        private string? _prevPipe;

        private bool _abortPipeline;
        private bool _finishedPipeline;

        private bool IsEndPipeline => _finishedPipeline || _abortPipeline || _pipects!.IsCancellationRequested;

        private CancellationTokenSource? _pipects;

        private readonly Dictionary<string,
             (Func<EventPipe<T>, CancellationToken, Task> pipehandle,
             bool aggregateTasks,
             List<PipeCondition<T>> precondhandle,
             List<PipeStatus> status,
             List<(string Id, Func<EventPipe<T>, CancellationToken, Task> TaskHandle, PipeCondition<T>? TaskCondition, string? NameTask, string? NameCondition)> tasks)> _pipes = new();

        private int _currentPipeIndex;

        #region ctor

        private PipeAndFilterControl()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit,
                "Invalid ctor PipeAndFilterControl");
        }

        public PipeAndFilterControl(CancellationToken cts)
        {
            _ctstoken = cts;
        }

        #endregion

        #region IPipelineInit

        IPipeAndFilterInit<T> IPipeAndFilterInit<T>.Logger(ILogger? value)
        {
            _logger = value??NullLogger.Instance;
            return this;
        }

        IPipeAndFilterInit<T> IPipeAndFilterInit<T>.CorrelationId(string? value)
        {
            _cid = value;
            return this;
        }

        IPipeAndFilter<T> IPipeAndFilterInit<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterInit<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterInit<T> IPipeAndFilterInit<T>.Init(T contract)
        {
            _contract = contract;
            return this;
        }

        IPipeAndFilterInit<T> IPipeAndFilterInit<T>.MaxDegreeProcess(int value)
        {
            if (value < 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "MaxDegreeProcess must be greater than zero");
            }
            _maxDegreeProcess = value;
            return this;
        }

        #endregion

        #region IPipeline

        IPipeAndFilter<T> IPipeAndFilter<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilter<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        async ValueTask<ResultPipeAndFilter<T>> IPipeAndFilter<T>.Run()
        {
            return await SharedRun(_ctstoken);
        }

        IPipeAndFilterConditions<T> IPipeAndFilter<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipelineConditions

        IPipeAndFilter<T> IPipeAndFilterConditions<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterConditions<T> IPipeAndFilterConditions<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        async ValueTask<ResultPipeAndFilter<T>> IPipeAndFilterConditions<T>.Run()
        {
            return await SharedRun(_ctstoken);
        }

        #endregion

        #region IPipelineTasks

        IPipeAndFilter<T> IPipeAndFilterTasks<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? name)
        {
            SharedAddTask(command,null, name, null);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? nametask, string? namecondition)
        {
            SharedAddTask(command, condition, nametask, namecondition);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        async ValueTask<ResultPipeAndFilter<T>> IPipeAndFilterTasks<T>.Run()
        {
            return await SharedRun(_ctstoken);
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

        private void SharedAddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias, bool agregatetask)
        {
            var id = Guid.NewGuid().ToString();
            if (command == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "command cannot be null");
            }
            if (alias == null) 
            {
                var seq = 1;
                var name = command.Method.Name;
                alias = name;
                while (_aliasToId.ContainsKey(alias))
                {
                    alias = $"{name}({seq})";
                }
            }
            if (!_aliasToId.TryAdd(alias, id))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit, 
                    "Alias already exists");
            }
            _idToAlias.Add(id, alias);
            
            if (!_pipes.TryAdd(id, (command, agregatetask, new(), new(), new())))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "idpipe already exists");
            }
            _currentPipe = id;
        }

        private void SharedAddTask(Func<EventPipe<T>, CancellationToken, Task> command, Func<EventPipe<T>, CancellationToken, ValueTask<bool>>? condition, string? nametask, string? namecond)
        {
            var id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(_currentPipe))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit, 
                    "Pipe not exist to add task");
            }
            if (!_pipes[_currentPipe].aggregateTasks)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit, 
                    "Pipe not aggregate tasks");
            }
            if (command == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit, 
                    "command cannot be null");
            }
            if (condition != null && string.IsNullOrEmpty(namecond))
            {
                namecond = condition.Method.Name;  
            }
            if (string.IsNullOrEmpty(nametask))
            {
                nametask = command.Method.Name;
            }
            PipeCondition<T>? pipecond = null;
            if (condition != null)
            {
                pipecond = new PipeCondition<T>(condition, null, nametask);
            }
            _pipes[_currentPipe].tasks.Add((id, command, pipecond, nametask, namecond));
        }

        private void SharedWithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            if (condition == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit, 
                    "condition cannot be null");
            }
            if (string.IsNullOrEmpty(namecondition))
            {
                namecondition = condition.Method.Name;
            }
            _pipes[_currentPipe!].precondhandle.Add(new PipeCondition<T>(condition, aliasgoto, namecondition));
        }

        private async ValueTask<ResultPipeAndFilter<T>> SharedRun(CancellationToken cts)
        {

            _pipects = CancellationTokenSource.CreateLinkedTokenSource(cts);

            InitControl();

            var aux = await ExecutePipes();

            Dispose();

            return aux;
        }

        private void InitControl()
        {
            if (_pipes.Count == 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit, 
                    "Not pipes to run");
            }
            _sequencePipes.AddRange(_pipes.Keys.ToArray());
            foreach (var item in _pipes.Where(x => x.Value.precondhandle.Any()))
            {
                foreach (var precond in item.Value.precondhandle.Where(x => !string.IsNullOrEmpty(x.GotoId)))
                {
                    var id = _aliasToId[precond.GotoId!];
                    if (!_pipes.ContainsKey(id))
                    {
                        throw new PipeAndFilterException(
                            PipeAndFilterException.StatusInit,
                            $"Condition {precond.Name ?? ""} with invalid go to pipe Alias: {precond.GotoId}");
                    }
                }
            }
        }

        private async ValueTask<ResultPipeAndFilter<T>> ExecutePipes()
        {
            _currentPipeIndex = 0;
            _currentPipe = _sequencePipes[0];
            while (!IsEndPipeline)
            {
                await NextPipe();
                if (!IsEndPipeline)
                {
                    var tm = Stopwatch.StartNew();
                    var elapsed = TimeSpan.Zero;
                    var sta = TaskStatus.WaitingForActivation;
                    try
                    {
                        _savedtaskvalues.Clear();
                        if (_pipes[_currentPipe!].aggregateTasks)
                        {
                            await ExecuteTasksPipes(_pipes[_currentPipe!].tasks!);
                        }
                        if (!IsEndPipeline)
                        {
                            string? aliasprev = null;
                            if (!string.IsNullOrEmpty(_prevPipe))
                            {
                                aliasprev = _idToAlias[_prevPipe];
                            };
                            string? aliascur = _idToAlias[_currentPipe!];
                            var evt = new EventPipe<T>(
                                _cid,
                                _logger,
                                ChangeContract!,
                                _savedvalues.ToImmutableArray(),
                                _savedtaskvalues.ToImmutableArray(),
                                _prevPipe,
                                _currentPipe!,
                                aliasprev, aliascur);
                            await _pipes[_currentPipe!].pipehandle(evt, _pipects!.Token);
                            sta = TaskStatus.RanToCompletion;
                            elapsed = tm.Elapsed;
                            EnsureResultEventPipe(_currentPipe!, evt);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        elapsed = tm.Elapsed;
                        sta = TaskStatus.Canceled;
                        _finishedPipeline = true;
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
                                _idToAlias[_currentPipe!],
                                null, true),
                            "Error handler Pipe",
                            ex);
                        _abortPipeline = true;
                        _finishedPipeline = true;
                    }
                    tm.Stop();
                    _pipes[_currentPipe!].status.Add(new PipeStatus(
                        HandlerType.Pipe,
                        sta,
                        elapsed,
                        _idToAlias[_currentPipe!],
                        null, true));

                }
                if (_lastexception != null)
                {
                    _pipects!.Cancel();
                }
                if (!IsEndPipeline)
                {
                    _currentPipeIndex++;
                    if (_currentPipeIndex < _sequencePipes.Count)
                    {
                        _prevPipe = _currentPipe;
                        _currentPipe = _sequencePipes[_currentPipeIndex];
                    }
                    else
                    {
                        _finishedPipeline = true;
                    }
                }
            }
            return new ResultPipeAndFilter<T>(
                _contract,
                _abortPipeline,
                _lastexception,
                _pipes.Select(x =>
                    new PipeRanStatus(
                        x.Key,
                        _idToAlias[x.Key],
                        x.Value.status)).ToImmutableArray()
                );
        }

        private async Task ExecuteTasksPipes(List<(string Id, Func<EventPipe<T>, CancellationToken, Task> TaskHandle, PipeCondition<T>? TaskCondition, string? NameTask, string? NameCondition)> tasks)
        {
            var i = 0;
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
                            aliasprev = _idToAlias[_prevPipe];
                        };
                        string? aliascur = _idToAlias[_currentPipe!];

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
                            _finishedPipeline = true;
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
                            _abortPipeline = true;
                            _finishedPipeline = true;
                            _pipects!.Cancel(false);
                        }
                        tm.Stop();
                        _pipes[_currentPipe!].status.Add(new PipeStatus(
                            HandlerType.ConditionTask,
                            sta,
                            elapsed,
                            tasks[i].NameTask,
                            null, isvalidtask));
                        if (IsEndPipeline)
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
                                    aliasprev = _idToAlias[_prevPipe];
                                };
                                aliascur = _idToAlias[_currentPipe!];
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
                                    _finishedPipeline = true;
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
                                    _abortPipeline = true;
                                    _finishedPipeline = true;
                                }
                            }
                            tm.Stop();
                            lock (_lockObj)
                            {
                                _pipes[_currentPipe!].status.Add(new PipeStatus(
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
                } while (!IsEndPipeline && i < tasks.Count && degreecount < _maxDegreeProcess);
                if (!IsEndPipeline)
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
            } while (!IsEndPipeline && i < tasks.Count);
        }

        private async Task NextPipe()
        {
            while (!IsEndPipeline)
            {
                var isok = true;
                foreach (var itemcond in _pipes[_currentPipe!].precondhandle)
                {
                    var condpipeType = string.IsNullOrEmpty(itemcond.GotoId) ? HandlerType.Condition : HandlerType.ConditionGoto;
                    var sta = TaskStatus.RanToCompletion;
                    string? aliasprev = null;
                    if (!string.IsNullOrEmpty(_prevPipe))
                    {
                        aliasprev = _idToAlias[_prevPipe];
                    };
                    string? aliascur = _idToAlias[_currentPipe!];

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
                        _finishedPipeline = true;
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
                        _abortPipeline = true;
                        _finishedPipeline = true;
                        _pipects!.Cancel(false);
                    }
                    tm.Stop();
                    _pipes[_currentPipe!].status.Add(new PipeStatus(
                        condpipeType,
                        sta,
                        elapsed,
                        itemcond.Name,
                        itemcond.GotoId, isok));
                    if (!IsEndPipeline)
                    {
                        if ((!isok && condpipeType == HandlerType.Condition) || (isok && condpipeType == HandlerType.ConditionGoto))
                        {
                            if (!string.IsNullOrEmpty(itemcond.GotoId))
                            {
                                _prevPipe = _currentPipe;
                                _currentPipe = _aliasToId[itemcond.GotoId];
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
                                _finishedPipeline = true;
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
                _finishedPipeline = true;
            }
            var alias = _idToAlias[idpipe];
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

        private void EnsureResultEventPipeTask(string idtask, string? name, EventPipe<T> eventPipe)
        {
            lock (_lockObj)
            {
                if (eventPipe.FinishedPipeAndFilter)
                {
                    _finishedPipeline = true;
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
