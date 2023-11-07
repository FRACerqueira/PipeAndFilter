// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************


using System.Collections.Immutable;

namespace PipeFilterCore
{
    internal interface IPipeAndFilterOptions<T> where T : class
    {
        string? ServiceId { get; }
        IImmutableDictionary<string, string> AliasToId { get; }
        IImmutableDictionary<string, string?> IdToAlias { get; }
        IImmutableDictionary<string, int> MaxDegreeProcess { get; }
        IImmutableList<PipeCommand<T>> Pipes { get; }
        IImmutableDictionary<string, bool> AggregateTasks { get; }
        IImmutableDictionary<string, IImmutableList<PipeCondition<T>>> PreConditions { get; }
        IImmutableDictionary<string, IImmutableList<PipeTask<T>>> Tasks { get; }
    }
}
