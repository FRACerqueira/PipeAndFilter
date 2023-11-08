// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using Microsoft.Extensions.DependencyInjection;

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the extensions to add PipeAndFilter to the ServiceCollection.
    /// </summary>
    public static class PipeAndFilterExtensions
    {
        /// <summary>
        /// Add PipeAndFilter to ServiceCollection.
        /// </summary>
        /// <typeparam name="T">Type of contract.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="pipeAndFilterServiceBuild">The PipeAndFilter</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddPipeAndFilter<T>(this IServiceCollection services, IPipeAndFilterService<T> pipeAndFilterServiceBuild) where T : class
        {
            return services.AddSingleton(pipeAndFilterServiceBuild);
        }
    }
}
