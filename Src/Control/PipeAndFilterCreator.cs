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
        /// Create PipeAndFilter component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sepsameinstance">
        /// The separator char when it has the same instance. Default value is '#'
        /// <br>When the alias refers to the same instance and the alias is not informed, an alias is created with the name of the activated method, the char and a sequence starting from 1.</br>
        /// </param>
        /// <returns><see cref="IPipeAndFilterStart{T}"/></returns>
        public static IPipeAndFilterStart<T> New<T>(char sepsameinstance = '#') where T : class
        {
            return new PipeAndFilterBuild<T>(sepsameinstance);
        }
    }
}