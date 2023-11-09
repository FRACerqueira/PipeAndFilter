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

        public PreCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>>? handle, string? gotoId, string? name)
        {
            Handle = handle;
            GotoId = gotoId;
            Name = name;
        }
        public Func<EventPipe<T>, CancellationToken, ValueTask<bool>>? Handle { get; }
        public string? GotoId { get;}
        public string? Name { get; }
    }
}
