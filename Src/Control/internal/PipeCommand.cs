namespace PipeFilterCore
{
    internal class PipeCommand<T> where T : class
    {
        public PipeCommand(string id,bool isagregate, Func<EventPipe<T>, CancellationToken, Task> handler) 
        {
            Id = id;
            IsAgregate = isagregate;
            MaxDegreeProcess = Environment.ProcessorCount;
            Handler = handler;
            Tasks = new();
            Condtitions = new();
        }
        public string Id { get; }
        public bool IsAgregate { get; }
        public int MaxDegreeProcess { get; set; }
        public List<PipeTask<T>> Tasks { get; }
        public List<PreCondition<T>> Condtitions { get; }
        public Func<EventPipe<T>, CancellationToken, Task> Handler { get; }
    }
}
