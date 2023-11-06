// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using PipeFilterCore.Control;

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
        /// <returns><see cref="IPipeAndFilterCreateService{T}"/></returns>
        public static IPipeAndFilterCreateService<T> New<T>() where T : class
        {
            return new PipeAndFilterBuild<T>();
        }
    }
}