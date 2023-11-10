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
        ImmutableDictionary<string, string> AliasToId { get; }
        ImmutableDictionary<string, string?> IdToAlias { get; }
        ImmutableList<PipeCommand<T>> Pipes { get; }
    }
}
