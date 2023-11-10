namespace PipeFilterCore
{
    internal class PipeCommand<T> where T : class
    {
        public PipeCommand(string id,bool isAggregate, Func<EventPipe<T>, CancellationToken, Task> handler) 
        {
            Id = id;
            IsAggregate = isAggregate;
            MaxDegreeProcess = Environment.ProcessorCount;
            Handler = handler;
            Tasks = new();
            Condtitions = new();
        }
        public string Id { get; }
        public bool IsAggregate { get; }
        public int MaxDegreeProcess { get; set; }
        public List<PipeTask<T>> Tasks { get; }
        public List<PreCondition<T>> Condtitions { get; }
        public Func<EventPipe<T>, CancellationToken, Task> Handler { get; }
        public PipeCommand<T>? HandlerAfter { get; set; }

    }
}
