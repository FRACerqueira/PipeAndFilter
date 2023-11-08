// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents PipeAndFilter Creator.
    /// </summary>
    public static class PipeAndFilter
    {
        /// <summary>
        /// Create PipeAndFilter service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><see cref="IPipeAndFilterStart{T}"/></returns>
        public static IPipeAndFilterStart<T> New<T>() where T : class
        {
            return new PipeAndFilterBuild<T>();
        }
    }
}