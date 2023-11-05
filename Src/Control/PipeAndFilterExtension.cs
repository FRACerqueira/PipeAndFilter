// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterPlus
{
    /// <summary>
    /// Represents PipeAndFilter Extension.
    /// </summary>
    public static class PipeAndFilter
    {
        /// <summary>
        /// Create PipeAndFilter.
        /// </summary>
        /// <param name="cts">The <see cref="CancellationToken"/></param>
        /// <typeparam name="T">Type of return.</typeparam>]
        /// <remarks>
        /// If <see cref="CancellationToken"/> ommited the value is 'CancellationToken.None'.
        /// </remarks>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        public static IPipeAndFilterInit<T> Create<T>(CancellationToken? cts = null) where T : class
        {
            return new PipeAndFilterControl<T>(cts??CancellationToken.None);
        }
    }
}