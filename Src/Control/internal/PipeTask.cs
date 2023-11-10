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

        public PipeTask(string id, Func<EventPipe<T>, CancellationToken, Task> handler, string? nameTask)
        {
            Id = id;
            Handler = handler;
            Name = nameTask;
            Condtitions = new();
        }
        public string Id { get; }
        public Func<EventPipe<T>, CancellationToken, Task> Handler { get; }
        public List<PreCondition<T>> Condtitions { get; }
        public string? Name { get; }
    }
}
