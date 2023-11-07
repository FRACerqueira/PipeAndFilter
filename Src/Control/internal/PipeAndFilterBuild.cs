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
        IPipeAndFilterCreateService<T>,
        IPipeAndFilterService<T>,
        IPipeAndFilterConditionsService<T>,
        IPipeAndFilterTasksService<T>,
        IPipeAndFilterOptions<T>
        where T : class
    {
        private readonly Dictionary<string, string> _aliasToId = new();
        private readonly Dictionary<string, string?> _idToAlias = new();
        private readonly Dictionary<string, int> _maxDegreeProcess = new();
        private readonly List<PipeCommand<T>> _pipes = new();
        private readonly Dictionary<string, bool> _aggregateTasks = new();
        private readonly Dictionary<string, List<PipeCondition<T>>> _precondhandler = new();
        private readonly Dictionary<string, List<PipeTask<T>>> _tasks = new();
        private readonly int _defaultMaxProcess = Environment.ProcessorCount;
        private string? _currentPipe;
        private string? _serviceId;


        #region IPipeAndFilterOptions

        public string? ServiceId => _serviceId;

        public IImmutableDictionary<string, string> AliasToId => _aliasToId.ToImmutableDictionary();

        public IImmutableDictionary<string, string?> IdToAlias => _idToAlias.ToImmutableDictionary();

        public IImmutableDictionary<string, int> MaxDegreeProcess => _maxDegreeProcess.ToImmutableDictionary();

        public IImmutableList<PipeCommand<T>> Pipes => _pipes.ToImmutableList();

        public IImmutableDictionary<string, bool> AggregateTasks => _aggregateTasks.ToImmutableDictionary();

        public IImmutableDictionary<string, IImmutableList<PipeCondition<T>>> PreConditions
        {
            get
            {
                return ImmutableDictionary
                    .CreateRange(_precondhandler.Select(obj => new KeyValuePair<string, IImmutableList<PipeCondition<T>>>(obj.Key, obj.Value.ToImmutableList())));
            }
        }

        public IImmutableDictionary<string, IImmutableList<PipeTask<T>>> Tasks
        {
            get
            {
                return ImmutableDictionary
                    .CreateRange(_tasks.Select(obj => new KeyValuePair<string, IImmutableList<PipeTask<T>>>(obj.Key, obj.Value.ToImmutableList())));
            }
        }

        #endregion

        #region IPipeAndFilterCreateService

        IPipeAndFilterService<T> IPipeAndFilterCreateService<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterCreateService<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        #endregion

        #region IPipeAndFilterService

        IPipeAndFilterService<T> IPipeAndFilterService<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterService<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        IPipeAndFilterConditionsService<T> IPipeAndFilterService<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            SharedWithCondition(condition,aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterConditionsService

        IPipeAndFilterService<T> IPipeAndFilterConditionsService<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterConditionsService<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        IPipeAndFilterConditionsService<T> IPipeAndFilterConditionsService<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterTasksService

        IPipeAndFilterTasksService<T> IPipeAndFilterTasksService<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, null, nametask, null);
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterTasksService<T>.AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? nametask, string? namecondition)
        {
            SharedAddTask(command, condition, nametask, namecondition);
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterTasksService<T>.MaxDegreeProcess(int value)
        {
            if (value < 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "MaxDegreeProcess must be greater than zero");
            }
            if (string.IsNullOrEmpty(_currentPipe))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe found to set MaxDegreeProcess");
            }
            if (!_aggregateTasks.TryGetValue(_currentPipe!, out _))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe not aggregate tasks");
            }
            if (!_maxDegreeProcess.TryGetValue(_currentPipe!, out _))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe not found to set MaxDegreeProcess");
            }
            _maxDegreeProcess[_currentPipe!] = value;
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterTasksService<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition)
        {
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        IPipeAndFilterService<T> IPipeAndFilterTasksService<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasksService<T> IPipeAndFilterTasksService<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        #endregion

        #region IPipeAndFilterBuild

        IPipeAndFilterServiceBuild<T> IPipeAndFilterBuild<T>.Build(string? serviceId)
        {
            _serviceId = serviceId;
            if (_pipes.Count == 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Not pipes to run");
            }
            foreach (var item in _precondhandler)
            {
                foreach (var precond in item.Value.Where(x => !string.IsNullOrEmpty(x.GotoId)))
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
            return new PipeAndFilterService<T>(serviceId, this);
        }

        IPipeAndFilterRunService<T> IPipeAndFilterBuild<T>.BuildAndCreate()
        {
            return new PipeAndFilterControl<T>(new PipeAndFilterService<T>(null, this));
        }


        #endregion

        private void SharedAddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias, bool agregatetask)
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
            if (agregatetask)
            {
                _maxDegreeProcess.Add(id, _defaultMaxProcess);
            }
            if (_pipes.Any(x => x.Id == id))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "idpipe already exists");
            }
            _pipes.Add(new PipeCommand<T>(id, command));

            if (!_aggregateTasks.TryAdd(id, agregatetask))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "idpipe already exists");
            }
            if (!_precondhandler.TryAdd(id, new()))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "idpipe already exists");
            }
            if (!_tasks.TryAdd(id, new()))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "idpipe already exists");
            }
            _currentPipe = id;
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
            if (_precondhandler.TryGetValue(_currentPipe!, out var precod))
            {
                precod.Add(new PipeCondition<T>(condition, aliasgoto, namecondition));
            }
            else
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Current Pipe not found");
            }
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
            if (!_aggregateTasks.TryGetValue(_currentPipe!, out _))
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
            if (_tasks.TryGetValue(_currentPipe!, out var tasks))
            {
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
                tasks.Add(new PipeTask<T>(id, command, pipecond, nametask, namecond));
            }
            else
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe not found to add task");
            }
        }

    }
}
