// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;

namespace PipeFilterCore
{

    /// <summary>
    /// Represents the commands of the PipeAndFilter definition.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    internal class PipeAndFilterBuild<T> :
        IPipeAndFilterAfterAggregate<T>,
        IPipeAndFilterAfterAggregateCondition<T>,
        IPipeAndFilterAfterPipe<T>,
        IPipeAndFilterAggregate<T>,
        IPipeAndFilterAggregateCondition<T>,
        IPipeAndFilterBuild<T>,
        IPipeAndFilterCondition<T>,
        IPipeAndFilterPipe<T>,
        IPipeAndFilterStart<T>,
        IPipeAndFilterOptions<T>
        where T : class
    {
        private PipeAndFilterBuild()
        {
        }

        public PipeAndFilterBuild(char sepsameinstance)
        {
            _sepsameinstance = sepsameinstance;       
        }

        private readonly char _sepsameinstance;
        private readonly Func<EventPipe<T>, CancellationToken, Task> _emptypipe = (_, _) => { return Task.CompletedTask; };
        private readonly Dictionary<string, string> _aliasToId = new();
        private readonly Dictionary<string, string?> _idToAlias = new();
        private readonly List<PipeCommand<T>> _pipes = new();
        private string? _currentPipe;
        private string? _currentPipeAfter;
        private string? _currentTask;
        private string? _serviceId;

        #region IPipeAndFilterOptions

        string? IPipeAndFilterOptions<T>.ServiceId => _serviceId;

        ImmutableDictionary<string, string> IPipeAndFilterOptions<T>.AliasToId => _aliasToId.ToImmutableDictionary();

        ImmutableDictionary<string, string?> IPipeAndFilterOptions<T>.IdToAlias => _idToAlias.ToImmutableDictionary();

        ImmutableList<PipeCommand<T>> IPipeAndFilterOptions<T>.Pipes => _pipes.ToImmutableList();

        #endregion

        #region IPipeAndFilterStart

        IPipeAndFilterPipe<T> IPipeAndFilterStart<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterStart<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterService<T> IPipeAndFilterBuild<T>.Build(string? serviceId)
        {
            _serviceId = serviceId;
            if (_pipes.Count == 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Not pipes to run");
            }

            foreach (var item in _pipes)
            {
                foreach (var precond in item.Condtitions.Where(x => !string.IsNullOrEmpty(x.GotoId)))
                {
                    var id = _aliasToId[precond.GotoId!];
                    if (!_pipes.Any(x => x.Id == id))
                    {
                        throw new PipeAndFilterException(
                            PipeAndFilterException.StatusInit,
                            $"Condition {precond.Name ?? ""} with invalid go to pipe Alias: {precond.GotoId}");
                    }
                }
                if (item.HandlerAfter != null)
                {
                    foreach (var precond in item.HandlerAfter!.Condtitions.Where(x => !string.IsNullOrEmpty(x.GotoId)))
                    {
                        var id = _aliasToId[precond.GotoId!];
                        if (!_pipes.Any(x => x.Id == id))
                        {
                            throw new PipeAndFilterException(
                                PipeAndFilterException.StatusInit,
                                $"Condition {precond.Name ?? ""} with invalid go to pipe Alias: {precond.GotoId}");
                        }
                    }
                }
            }
            return new PipeAndFilterService<T>(serviceId, this);
        }

        IPipeAndFilterInit<T> IPipeAndFilterBuild<T>.BuildAndCreate()
        {
            return new PipeAndFilterControl<T>(new PipeAndFilterService<T>(null, this));
        }

        #endregion

        #region IPipeAndFilterPipe

        IPipeAndFilterPipe<T> IPipeAndFilterPipe<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterPipe<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterPipe<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition,null, namecondition);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterPipe<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            SharedAddWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        IPipeAndFilterAfterPipe<T> IPipeAndFilterPipe<T>.AfterRunningPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command,alias, false);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterPipe<T>.AfterRunningAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, true);
            return this;
        }

        #endregion

        #region IPipeAndFilterCondition

        IPipeAndFilterPipe<T> IPipeAndFilterCondition<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAfterPipe<T> IPipeAndFilterCondition<T>.AfterRunningPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, false);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterCondition<T>.AfterRunningAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, true);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterCondition<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterCondition<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition,null,namecondition);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterCondition<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            SharedAddWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterAggregateCondition

        IPipeAndFilterPipe<T> IPipeAndFilterAggregateCondition<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command,alias, false);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAggregateCondition<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterAfterPipe<T> IPipeAndFilterAggregateCondition<T>.AfterRunning(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, false);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAggregateCondition<T>.AfterRunningAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, true);
            return this;
        }

        IPipeAndFilterAggregateCondition<T> IPipeAndFilterAggregateCondition<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterAggregateCondition<T> IPipeAndFilterAggregateCondition<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition, null, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterAggregate

        IPipeAndFilterPipe<T> IPipeAndFilterAggregate<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAggregate<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAggregate<T>.AfterRunningAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, true);
            return this;
        }

        IPipeAndFilterAfterPipe<T> IPipeAndFilterAggregate<T>.AfterRunningPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipeAfter(command, alias, false);
            return this;
        }


        IPipeAndFilterAggregate<T> IPipeAndFilterAggregate<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAggregate<T>.MaxDegreeProcess(int value)
        {
            SharedMaxDegreeProcess(value);
            return this;
        }

        IPipeAndFilterAggregateCondition<T> IPipeAndFilterAggregate<T>.AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAggregate<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition,null,namecondition);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAggregate<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            SharedAddWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterAfterPipe

        IPipeAndFilterPipe<T> IPipeAndFilterAfterPipe<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAfterPipe<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterAfterPipe<T> IPipeAndFilterAfterPipe<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition,null, namecondition);
            return this;
        }

        IPipeAndFilterAfterPipe<T> IPipeAndFilterAfterPipe<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            SharedAddWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterAfterAggregateCondition

        IPipeAndFilterPipe<T> IPipeAndFilterAfterAggregateCondition<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAggregate<T> IPipeAndFilterAfterAggregateCondition<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAfterAggregateCondition<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterAfterAggregateCondition<T> IPipeAndFilterAfterAggregateCondition<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition,null,namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterAfterAggregate

        IPipeAndFilterAggregate<T> IPipeAndFilterAfterAggregate<T>.AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, true);
            return this;
        }

        IPipeAndFilterPipe<T> IPipeAndFilterAfterAggregate<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias)
        {
            SharedAddPipe(command, alias, false);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAfterAggregate<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAfterAggregate<T>.MaxDegreeProcess(int value)
        {
            SharedMaxDegreeProcess(value);
            return this;
        }

        IPipeAndFilterAfterAggregateCondition<T> IPipeAndFilterAfterAggregate<T>.AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAfterAggregate<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedAddWithCondition(condition,null,namecondition);
            return this;
        }

        IPipeAndFilterAfterAggregate<T> IPipeAndFilterAfterAggregate<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            SharedAddWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        private void SharedAddPipeAfter(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias, bool Aggregatetask)
        {
            var id = Guid.NewGuid().ToString();
            command ??= _emptypipe;
            if (alias == null)
            {
                var seq = 0;
                var name = command.Method.Name;
                alias = name;
                while (_aliasToId.ContainsKey(alias))
                {
                    seq++;
                    alias = $"{name}{_sepsameinstance}{seq}";
                }
            }
            if (!_aliasToId.TryAdd(alias, id))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Alias already exists");
            }
            if (!_idToAlias.TryAdd(id, alias))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "pipe already exists");
            }

            var index = _pipes.FindIndex(x => x.Id == _currentPipe!);
            _pipes[index].HandlerAfter = new PipeCommand<T>(id, Aggregatetask, command);
            _currentPipeAfter = id;
            _currentTask = null;
        }

        private void SharedAddPipe(Func<EventPipe<T>, CancellationToken, Task>? command, string? alias, bool Aggregatetask)
        {
            var id = Guid.NewGuid().ToString();
            if (command == null)
            {
                command = _emptypipe;
                var seq = 0;
                var name = "EmptyHandler";
                alias = name;
                while (_aliasToId.ContainsKey(alias))
                {
                    seq++;
                    alias = $"{name}{_sepsameinstance}{seq}";
                }
            }
            if (alias == null)
            {
                var seq = 0;
                var name = command.Method.Name;
                alias = name;
                while (_aliasToId.ContainsKey(alias))
                {
                    seq++;
                    alias = $"{name}({seq})";
                }
            }
            if (!_aliasToId.TryAdd(alias, id))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Alias already exists");
            }
            if (!_idToAlias.TryAdd(id,alias))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "pipe already exists");
            }

            _pipes.Add(new PipeCommand<T>(id, Aggregatetask, command));

            _currentPipe = id;
            _currentPipeAfter = null;
            _currentTask = null;

        }

        private void SharedAddWithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            var id = Guid.NewGuid().ToString();
            if (condition == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "condition cannot be null");
            }
            if (namecondition == null)
            {
                var seq = 0;
                var name = condition.Method.Name;
                namecondition = name;
                while (_aliasToId.ContainsKey(namecondition))
                {
                    seq++;
                    namecondition = $"{name}{_sepsameinstance}{seq}";
                }
            }
            if (!_aliasToId.TryAdd(namecondition, id))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Alias already exists");
            }
            if (!_idToAlias.TryAdd(id, namecondition))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "pipe already exists");
            }
            var index = _pipes.FindIndex(x => x.Id == _currentPipe!);
            if (_currentPipeAfter != null)
            {
                if (_currentTask != null)
                {
                    var indextask = _pipes[index].HandlerAfter!.Tasks.FindIndex(x => x.Id == _currentTask!);
                    _pipes[index].HandlerAfter!.Tasks[indextask].Condtitions.Add(new PreCondition<T>(id,condition, aliasgoto, namecondition));
                }
                else
                {
                    _pipes[index].HandlerAfter!.Condtitions.Add(new PreCondition<T>(id, condition, aliasgoto, namecondition));
                }
            }
            else
            {
                if (_currentTask != null)
                {
                    var indextask = _pipes[index].Tasks.FindIndex(x => x.Id == _currentTask!);
                    _pipes[index].Tasks[indextask].Condtitions.Add(new PreCondition<T>(id, condition, aliasgoto, namecondition));
                }
                else
                {
                    _pipes[index].Condtitions.Add(new PreCondition<T>(id, condition, aliasgoto, namecondition));
                }
            }
        }

        private void SharedAddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            var id = Guid.NewGuid().ToString();
            if (command == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "command cannot be null");
            }
            _currentTask = id;
            if (string.IsNullOrEmpty(nametask))
            {
                var seq = 0;
                var name = command.Method.Name;
                nametask = name;
                while (_aliasToId.ContainsKey(nametask))
                {
                    seq++;
                    nametask = $"{name}{_sepsameinstance}{seq}";
                }
            }
            if (!_aliasToId.TryAdd(nametask, id))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Alias already exists");
            }
            if (!_idToAlias.TryAdd(id, nametask))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "pipe already exists");
            }
            var index = _pipes.FindIndex(x => x.Id == _currentPipe!);
            if (_currentPipeAfter != null)
            {
                _pipes[index].HandlerAfter!.Tasks.Add(new PipeTask<T>(_currentTask, command, nametask));
            }
            else
            {
                _pipes[index].Tasks.Add(new PipeTask<T>(_currentTask, command, nametask));
            }
        }

        private void SharedMaxDegreeProcess(int value)
        {
            if (value < 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "MaxDegreeProcess must be greater than zero");
            }
            var index = _pipes.FindIndex(x => x.Id == _currentPipe!);
            if (_currentPipeAfter != null)
            {
                _pipes[index].HandlerAfter!.MaxDegreeProcess = value;
            }
            else
            {
                _pipes[index].MaxDegreeProcess = value;
            }
        }

    }
}
