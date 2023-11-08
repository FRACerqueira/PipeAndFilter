// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************
namespace PipeFilterCore
{

    /// <summary>
    /// Represents the commands for create a instance.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>

    public interface IPipeAndFilterService<T> where T : class
    {
        /// <summary>
        /// The service id for this type.
        /// </summary>
        string? ServiceId { get; }

        /// <summary>
        /// Create a instance.
        /// </summary>
        /// <returns><see cref="IPipeAndFilterInit{T}"/></returns>
        IPipeAndFilterInit<T> Create();
    }
}
