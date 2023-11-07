// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents commands for conditions.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public interface IPipeAndFilterConditionsService<T>: IPipeAndFilterBuild<T> where T : class
    {
        /// <summary>
        /// Add new pipe.
        /// </summary>
        /// <param name="command">The handler pipe to execute.</param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// <br>Alias ​​is used to reference in another pipe.</br>
        /// </param>
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
        /// <br>Alias ​​is used to reference in another pipe.</br>
        /// </param>
        /// <returns><see cref="IPipeAndFilterTasksService{T}"/></returns>
        IPipeAndFilterTasksService<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

        /// <summary>
        /// Add new condition.
        /// </summary>
        /// <param name="condition">The handle condition to execute.</param>
        /// <param name="aliasgoto">
        /// The alias to another pipe.
        /// <br>If condition not have link to another pipe, the value must be null.</br>
        /// </param>
        /// <param name="namecondition">The name for condition(optional).</param>
        /// <returns><see cref="IPipeAndFilterConditionsService{T}"/></returns>
        IPipeAndFilterConditionsService<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition = null);
    }

}
