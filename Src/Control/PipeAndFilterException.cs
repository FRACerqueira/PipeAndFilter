// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Runtime.Serialization;

namespace PipeAndFilter
{
    ///<inheritdoc cref="Exception"/>
    /// <summary>
    /// Represents a exception for Pipeline control
    /// </summary>
    [Serializable]
    public class PipeAndFilterException : Exception
    {
        ///<inheritdoc cref="Exception"/>
        public PipeAndFilterException()
        {
        }

        ///<inheritdoc cref="Exception"/>
        public PipeAndFilterException(string? message) : base(message)
        {
        }

        ///<inheritdoc cref="Exception"/>
        public PipeAndFilterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        ///<inheritdoc cref="Exception"/>
        protected PipeAndFilterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}