// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the commands for pipes / aggregate pipe.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public interface IPipeAndFilterPipe<T> : IPipeAndFilterBuild<T> where T : class
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

        /// <summary>
        /// Add new condition.
        /// </summary>
        /// <param name="condition">The handle to execute.</param>
        /// <param name="namecondition">The name for condition(optional).</param>
        /// <returns><see cref="IPipeAndFilterCondition{T}"/></returns>
        IPipeAndFilterCondition<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? namecondition = null);

        /// <summary>
        /// Add new go to condition.
        /// <br>If the condition is true, jump to the given pipe without executing the current pipe.</br>
        /// <br>If the false condition continues.</br>
        /// </summary>
        /// <param name="condition">The handle to execute.</param>
        /// <param name="aliasgoto">
        /// The alias to another pipe.
        /// </param>
        /// <param name="namecondition">The name for condition(optional).</param>
        /// <returns><see cref="IPipeAndFilterCondition{T}"/></returns>
        IPipeAndFilterCondition<T> WithGotoCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string aliasgoto, string? namecondition = null);


        /// <summary>
        /// Add new pipe to run after pipe completes.
        /// </summary>
        /// <param name="command">The handler to execute.
        /// <br>The handler command will run after all tasks are executed.</br>
        /// </param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// <br>Alias ​​is used to reference in another pipe.</br>
        /// </param>
        /// <returns><see cref="IPipeAndFilterAfterPipe{T}"/></returns>
        IPipeAndFilterAfterPipe<T> AfterRunningPipe(Func<EventPipe<T>, CancellationToken, Task>? command = null, string? alias = null);

        /// <summary>
        /// Add new Aggregate pipe to run after pipe/Aggregate pipe completes.
        /// </summary>
        /// <param name="command">The handler to execute.
        /// <br>The handler command will run after all tasks are executed.</br>
        /// </param>
        /// <param name="alias">
        /// The unique alias for pipe.
        /// <br>If the alias is omitted, the alias will be the handler name followed by the reference quantity (if any).</br>
        /// <br>Alias ​​is used to reference in another pipe.</br>
        /// </param>
        /// <returns><see cref="IPipeAndFilterAfterAggregate{T}"/></returns>
        IPipeAndFilterAfterAggregate<T> AfterRunningAggregatePipe(Func<EventPipe<T>, CancellationToken, Task>? command = null, string? alias = null);
    }
}
