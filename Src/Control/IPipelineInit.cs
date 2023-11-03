// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents control commands for initialization
    /// </summary>
    /// <typeparam name="T">Type of contract</typeparam>
    public interface IPipelineInit<T> where T : class
    {
        /// <summary>
        /// Initial contract value
        /// </summary>
        /// <param name="contract">The contract</param>
        /// <returns><see cref="IPipelineInit{T}"/></returns>
        IPipelineInit<T> Init(T contract);

        /// <summary>
        /// Maximum number of concurrent tasks enable. Default vaue is number of processors.
        /// </summary>
        /// <param name="value">Number of concurrent tasks</param>
        /// <returns><see cref="IPipelineInit{T}"/></returns>
        IPipelineInit<T> MaxDegreeProcess(int value);


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

    }
}