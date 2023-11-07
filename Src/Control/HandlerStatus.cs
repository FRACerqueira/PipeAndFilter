// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents a handler status.
    /// </summary>
    public enum HandlerStatus
    {
        /// <summary>
        /// Handler waiting to execute
        /// </summary>
        Created,
        /// <summary>
        /// Handler Completed
        /// </summary>
        Completed,
        /// <summary>
        /// Handler Completed
        /// </summary>
        Canceled,
        /// <summary>
        /// Handler Faulted
        /// </summary>
        Faulted
    }
}
