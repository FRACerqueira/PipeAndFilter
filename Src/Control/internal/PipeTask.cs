// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    internal readonly struct PipeTask<T>  where T: class
    {
        public PipeTask()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit,
                "Invalid ctor PipeTask");
        }

        public PipeTask(string id, Func<EventPipe<T>, CancellationToken, Task> handler, PipeCondition<T>? condition, string? nameTask, string? nameCondition)
        {
            Id = id;
            Handler = handler;
            Condition = condition;
            NameTask = nameTask;
            NameCondition = nameCondition;
        }
        public string Id { get; }
        public Func<EventPipe<T>, CancellationToken, Task> Handler { get; }
        public PipeCondition<T>? Condition { get; }
        public string? NameTask { get; }
        public string? NameCondition { get; }
    }
}
