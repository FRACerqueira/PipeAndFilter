// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore.Control
{
    /// <summary>
    /// Represents the commands of the PipeAndFilter definition.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    internal class PipeAndFilterBuild<T> :
        IPipeAndFilterCreateService<T>,
        IPipeAndFilterService<T>,
        IPipeAndFilterConditionsService<T>,
        IPipeAndFilterTasksService<T>
        where T : class
    {
        internal readonly Dictionary<string, string> _aliasToId = new();
        internal readonly Dictionary<string, string?> _idToAlias = new();
        internal readonly Dictionary<string, int> _maxDegreeProcess = new();
        internal readonly Dictionary<string,
             (Func<EventPipe<T>, CancellationToken, Task> pipehandle,
             bool aggregateTasks,
             List<PipeCondition<T>> precondhandle,
             List<PipeStatus> status,
             List<(string Id, Func<EventPipe<T>, CancellationToken, Task> TaskHandle, PipeCondition<T>? TaskCondition, string? NameTask, string? NameCondition)> tasks)> _pipes = new();

        private readonly int _defaultMaxProcess = Environment.ProcessorCount;
        private string? _currentPipe;

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
                    "Pipe not exist to set MaxDegreeProcess");
            }
            if (!_pipes[_currentPipe].aggregateTasks)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Pipe not aggregate tasks");
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
            if (_pipes.Count == 0)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "Not pipes to run");
            }
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

            if (agregatetask)
            {
                _maxDegreeProcess.Add(id, _defaultMaxProcess);
            }

            if (!_pipes.TryAdd(id, (command, agregatetask, new(), new(), new())))
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
            _pipes[_currentPipe!].precondhandle.Add(new PipeCondition<T>(condition, aliasgoto, namecondition));
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

    }
}
