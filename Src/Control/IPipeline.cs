// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents control commands for pipe
    /// </summary>
    /// <typeparam name="T">Type of contract</typeparam>
    public interface IPipeline<T> where T : class
    {
        /// <summary>
        /// Add new pipe
        /// </summary>
        /// <param name="command">The handler pipe to execute</param>
        /// <param name="alias">The unique alias for pipe (optional).
        /// <br>Tip: Alias is used for go to operation</br>
        /// </param>
        /// <returns><see cref="IPipeline{T}"/></returns>
        IPipeline<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

        /// <summary>
        /// Add new pipe aggregate tasks.
        /// <br>The handler command will run after all tasks are executed.</br>
        /// </summary>
        /// <param name="command">The handler pipe aggregate to execute</param>
        /// <param name="alias">The unique alias for pipe (optional).
        /// <br>Tip: Alias is used for go to operation</br>
        /// </param>
        /// <returns><see cref="IPipeline{T}"/></returns>
        IPipelineTasks<T> AddPipeTasks(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

        /// <summary>
        /// Add new condition for run this pipe
        /// </summary>
        /// <param name="condition">The handle condition to execute</param>
        /// <param name="aliasgoto">The alias go to handle.
        /// <br>Tip: If condition not have go to, the value must be null</br>
        /// </param>
        /// <param name="namecondition">The name for condition(optional).</param>
        /// <returns><see cref="IPipelineConditions{T}"/></returns>
        IPipelineConditions<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition = null);


        /// <summary>
        /// Execute the pipeline
        /// </summary>
        /// <returns><see cref="ResultPipeline{T}"/></returns>
        ValueTask<ResultPipeline<T>> Run();
    }
}