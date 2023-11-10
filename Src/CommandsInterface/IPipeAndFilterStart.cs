// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the commands for Pipes.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public interface IPipeAndFilterStart<T>: IPipeAndFilterBuild<T> where T : class
    {
        /// <summary>
        /// Add new pipe.
        /// </summary>
        /// <param name="command">The handler to execute.</param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// <br>Alias ​​is used to reference in another pipe.</br>
        /// </param>
        /// <returns><see cref="IPipeAndFilterPipe{T}"/></returns>
        IPipeAndFilterPipe<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task>? command = null, string? alias = null);

        /// <summary>
        /// Add new aggregate pipe.
        /// </summary>
        /// <param name="command">The handler to execute.
        /// <br>The handler command will run after all tasks are executed.</br>
        /// </param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// <br>Alias ​​is used to reference in another pipe.</br>
        /// </param>
        /// <returns><see cref="IPipeAndFilterAggregate{T}"/></returns>
        IPipeAndFilterAggregate<T> AddAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command = null, string? alias = null);
    }
}
