// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents PipeAndFilter Extension
    /// </summary>
    public static class Pipeline
    {
        /// <summary>
        /// Create Pipeline control
        /// </summary>
        /// <typeparam name="T">Type of return</typeparam>
        /// <returns><see cref="IPipelineInit{T}"/></returns>
        public static IPipelineInit<T> Create<T>(CancellationToken? cts = null) where T : class
        {
            return new PipeAndFilterControl<T>(cts??CancellationToken.None);
        }
    }
}