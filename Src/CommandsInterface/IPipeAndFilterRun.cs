// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the command for Run.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public interface IPipeAndFilterRun<T> where T : class
    {
        /// <summary>
        /// Execute PipeAndFilter.
        /// </summary>
        /// <returns><see cref="ResultPipeAndFilter{T}"/></returns>
        ValueTask<ResultPipeAndFilter<T>> Run(CancellationToken? cancellation = null);
    }
}
