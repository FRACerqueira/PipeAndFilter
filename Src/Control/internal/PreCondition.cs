// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    internal readonly struct PreCondition<T> where T : class
    {
        public PreCondition()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit, 
                "Invalid ctor PipeCondition");
        }

        public PreCondition(string id, Func<EventPipe<T>, CancellationToken, ValueTask<bool>>? handle, string? gotoId, string? name)
        {
            Id = id;
            Handle = handle;
            GotoId = gotoId;
            Name = name;
        }
        public string Id { get; }
        public Func<EventPipe<T>, CancellationToken, ValueTask<bool>>? Handle { get; }
        public string? GotoId { get;}
        public string? Name { get; }
    }
}
