// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents Pipeline control commands for task
    /// </summary>
    /// <typeparam name="T">Type of contract</typeparam>
    public interface IPipelineTasks<T> where T : class
    {
        /// <summary>
        /// End tasks and add new pipe
        /// </summary>
        /// <param name="command">The handler pipe to execute</param>
        /// <param name="alias">The unique alias for pipe (optional).
        /// <br>Tip: Alias is used for go to operation</br>
        /// </param>
        /// <returns><see cref="IPipeline{T}"/></returns>
        IPipeline<T> AddPipe(Func<EventPipe<T>, CancellationToken, Task> command, string? alias = null);

        /// <summary>
        /// Add new condition for run Aggregate pipe
        /// </summary>
        /// <param name="condition">The handle condition to execute</param>
        /// <param name="aliasgoto">The alias go to handle.
        /// <br>Tip: If condition not have go to, the value must be null</br>
        /// </param>
        /// <param name="namecondition">The name for condition(optional)</param>
        /// <returns><see cref="IPipelineTasks{T}"/></returns>
        IPipelineTasks<T> WithCondition(Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? aliasgoto, string? namecondition = null);

        /// <summary>
        /// Add new task for pararel run over pipe.
        /// </summary>
        /// <param name="command">The handler task to execute</param>
        /// <param name="nametask">The name for task (optional)</param>
        /// <returns><see cref="IPipelineTasks{T}"/></returns>
        IPipelineTasks<T> AddTask(Func<EventPipe<T>, CancellationToken, Task> command, string? nametask = null);

        /// <summary>
        /// Add new task for pararel run over pipe with condition.
        /// </summary>
        /// <param name="command">The handler task to execute</param>
        /// <param name="condition">The handler task to condition</param>
        /// <param name="nametask">The name for task (optional)</param>
        /// <param name="namecondition">The name for condition (optional)</param>
        /// <returns><see cref="IPipelineTasks{T}"/></returns>
        IPipelineTasks<T> AddTaskCondition(Func<EventPipe<T>, CancellationToken, Task> command, Func<EventPipe<T>, CancellationToken, ValueTask<bool>> condition, string? nametask = null, string? namecondition = null);


        /// <summary>
        /// Execute the pipeline
        /// </summary>
        /// <returns><see cref="ResultPipeline{T}"/></returns>
        ValueTask<ResultPipeline<T>> Run();
    }
}