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
    public interface IPipeAndFilterCreateService<T>: IPipeAndFilterBuild<T> where T : class
    {
        /// <summary>
        /// Add new pipe.
        /// </summary>
        /// <param name="command">The handler pipe to execute.</param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// </param>
        /// <remarks>Alias ​​is used to reference in another pipe.</remarks>
        /// <returns><see cref="IPipeAndFilterService{T}"/></returns>
        IPipeAndFilterService<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

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
        /// <returns><see cref="IPipeAndFilterTasksService{T}"/></returns>
        IPipeAndFilterTasksService<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);
    }
}
