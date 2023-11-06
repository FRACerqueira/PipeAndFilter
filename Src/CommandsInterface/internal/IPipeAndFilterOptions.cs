// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************


namespace PipeFilterCore.CommandsInterface
{
    internal interface IPipeAndFilterOptions<T> where T : class
    {
        string? ServiceId { get; }
        Dictionary<string, string> AliasToId { get; }
        Dictionary<string, string?> IdToAlias { get; }
        Dictionary<string, int> MaxDegreeProcess { get; }
        Dictionary<string,
             (Func<EventPipe<T>, CancellationToken, Task> pipehandle,
             bool aggregateTasks,
             List<PipeCondition<T>> precondhandle,
             List<PipeStatus> status,
             List<(string Id, Func<EventPipe<T>, CancellationToken, Task> TaskHandle, PipeCondition<T>? TaskCondition, string? NameTask, string? NameCondition)> tasks)> Pipes { get; }

    }
}
