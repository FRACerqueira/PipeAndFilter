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
        IPipeAndFilterStart<T>,
        IPipeAndFilterAdd<T>,
        IPipeAndFilterCondition<T>,
        IPipeAndFilterTasks<T>,
        IPipeAndFilterTaskCondition<T>,
        IPipeAndFilterOptions<T>
        where T : class
    {
        private readonly Dictionary<string, string> _aliasToId = new();
        private readonly Dictionary<string, string?> _idToAlias = new();
        private readonly List<PipeCommand<T>> _pipes = new();
        private string? _currentPipe;
        private string? _currentTask;
        private string? _serviceId;


        #region IPipeAndFilterOptions

        public string? ServiceId => _serviceId;

        public ImmutableDictionary<string, string> AliasToId => _aliasToId.ToImmutableDictionary();

        public ImmutableDictionary<string, string?> IdToAlias => _idToAlias.ToImmutableDictionary();

        public ImmutableList<PipeCommand<T>> Pipes => _pipes.ToImmutableList();

        #endregion

        #region IPipeAndFilterStart

        IPipeAndFilterAdd<T> IPipeAndFilterStart<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterStart<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        #endregion

        #region IPipeAndFilterAdd

        IPipeAndFilterAdd<T> IPipeAndFilterAdd<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterAdd<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterAdd<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedWithCondition(condition,null, namecondition);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterAdd<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            if (aliasgoto == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "aliasgoto cannot be null");
            }
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterCondition

        IPipeAndFilterAdd<T> IPipeAndFilterCondition<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterCondition<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterCondition<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedWithCondition(condition, null, namecondition);
            return this;
        }

        IPipeAndFilterCondition<T> IPipeAndFilterCondition<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            if (aliasgoto == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "aliasgoto cannot be null");
            }
            SharedWithCondition(condition, aliasgoto, namecondition);
            return this;
        }

        #endregion

        #region IPipeAndFilterTasks

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterTaskCondition<T> IPipeAndFilterTasks<T>.AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.MaxDegreeProcess(int value)
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
            var index = _pipes.FindIndex(x => x.Id == _currentPipe! && x.IsAgregate);
            if (index < 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe not aggregate tasks");
            }
            _pipes[index].MaxDegreeProcess = value;
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            SharedWithCondition(condition, null, namecondition);
            return this;
        }


        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition)
        {
            if (aliasgoto == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "aliasgoto cannot be null");
            }
            SharedWithCondition(condition, null, namecondition);
            return this;
        }

        IPipeAndFilterAdd<T> IPipeAndFilterTasks<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTasks<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        #endregion

        #region IPipeAndFilterTaskCondition

        IPipeAndFilterAdd<T> IPipeAndFilterTaskCondition<T>.AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, false);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTaskCondition<T>.AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias)
        {
            SharedAddPipeTasks(command, alias, true);
            return this;
        }

        IPipeAndFilterTasks<T> IPipeAndFilterTaskCondition<T>.AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            SharedAddTask(command, nametask);
            return this;
        }

        IPipeAndFilterTaskCondition<T> IPipeAndFilterTaskCondition<T>.WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition)
        {
            if (string.IsNullOrEmpty(_currentTask))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Task not exist to add WithCondition");
            }
            if (string.IsNullOrEmpty(namecondition))
            {
                namecondition = condition.Method.Name;
            }
            var index = _pipes.FindIndex(x => x.Id == _currentPipe! && x.IsAgregate);
            var indextask = _pipes[index].Tasks.FindIndex(x => x.Id == _currentTask!);
            _pipes[index].Tasks[indextask].Condtitions.Add(new PreCondition<T>(condition, null, namecondition));
            return this;
        }

        #endregion

        #region IPipeAndFilterBuild

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
            }
            return new PipeAndFilterService<T>(serviceId, this);
        }

        IPipeAndFilterInit<T> IPipeAndFilterBuild<T>.BuildAndCreate()
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

            _pipes.Add(new PipeCommand<T>(id, agregatetask, command));

            _currentPipe = id;
            _currentTask = null;

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
            var index = _pipes.FindIndex(x => x.Id == _currentPipe!);
            _pipes[index].Condtitions.Add(new PreCondition<T>(condition, aliasgoto, namecondition));
        }

        private void SharedAddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask)
        {
            var id = Guid.NewGuid().ToString();
            if (string.IsNullOrEmpty(_currentPipe))
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe not exist to add task");
            }
            if (command == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "command cannot be null");
            }
            _currentTask = id;
            if (string.IsNullOrEmpty(nametask))
            {
                nametask = command.Method.Name;
            }
            var index = _pipes.FindIndex(x => x.Id == _currentPipe! && x.IsAgregate);
            _pipes[index].Tasks.Add(new PipeTask<T>(_currentTask, command, nametask));
        }

    }
}
