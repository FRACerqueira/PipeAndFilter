// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the commands for build a service and Create the instance to run.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public interface IPipeAndFilterBuild<T> where T : class
    {
        /// <summary>
        /// Build PipeAndFilter to add into ServiceCollection.
        /// </summary>
        /// <param name="serviceId">The service Id.</param>
        /// <returns><see cref="IPipeAndFilterService{T}"/></returns>
        IPipeAndFilterService<T> Build(string? serviceId = null);

        /// <summary>
        /// Build and create PipeAndFilter to run.
        /// </summary>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        IPipeAndFilterInit<T> BuildAndCreate();
    }
}