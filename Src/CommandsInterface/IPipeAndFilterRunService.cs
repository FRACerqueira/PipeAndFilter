// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using Microsoft.Extensions.Logging;

namespace PipeFilterCore
{
    /// <summary>
    /// Represents commands for initialization and run.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public interface IPipeAndFilterRunService<T>: IPipeAndFilterRun<T> where T : class
    {
        /// <summary>
        /// The service id for this type.
        /// </summary>
        string? ServiceId { get; }

        /// <summary>
        /// Initial contract value.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns><see cref="IPipeAndFilterRunService{T}"/></returns>
        IPipeAndFilterRunService<T> Init(T contract);

        /// <summary>
        /// The Correlation Id.
        /// </summary>
        /// <param name="value">Correlation Id value.</param>
        /// <returns><see cref="IPipeAndFilterRunService{T}"/></returns>
        IPipeAndFilterRunService<T> CorrelationId(string? value);

        /// <summary>
        /// The logger handler.
        /// </summary>
        /// <param name="value">logger handler value.</param>
        /// <returns><see cref="IPipeAndFilterRunService{T}"/></returns>
        IPipeAndFilterRunService<T> Logger(ILogger? value);

    }
}