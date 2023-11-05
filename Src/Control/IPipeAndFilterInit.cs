// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using Microsoft.Extensions.Logging;

namespace PipeFilterCore
{
    /// <summary>
    /// Represents command for initialization
    /// </summary>
    /// <typeparam name="T">Type of contract</typeparam>
    public interface IPipeAndFilterInit<T> where T : class
    {
        /// <summary>
        /// Initial contract value.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        IPipeAndFilterInit<T> Init(T contract);

        /// <summary>
        /// Maximum number of concurrent tasks enable. 
        /// </summary>
        /// <param name="value">Number of concurrent tasks.</param>
        /// <remarks>
        /// The default value is number of processors.
        /// </remarks>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        IPipeAndFilterInit<T> MaxDegreeProcess(int value);

        /// <summary>
        /// The Correlation Id
        /// </summary>
        /// <param name="value">Correlation Id value</param>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        IPipeAndFilterInit<T> CorrelationId(string? value);

        /// <summary>
        /// The logger handler
        /// </summary>
        /// <param name="value">logger handler value</param>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        IPipeAndFilterInit<T> Logger(ILogger? value);


        /// <summary>
        /// Add new pipe.
        /// </summary>
        /// <param name="command">The handler pipe to execute.</param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// </param>
        /// <remarks>Alias ​​is used to reference in another pipe.</remarks>
        /// <returns><see cref="IPipeAndFilter{T}"/></returns>
        IPipeAndFilter<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

        /// <summary>
        /// Add new pipe aggregate tasks.
        /// </summary>
        /// <param name="command">The handler pipe aggregate to execute.
        /// <br>The handler command will run after all tasks are executed.</br>
        /// </param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// </param>
        /// <remarks>Alias ​​is used to reference in another pipe.</remarks>
        /// <returns><see cref="IPipeAndFilter{T}"/></returns>
        IPipeAndFilterTasks<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

    }
}