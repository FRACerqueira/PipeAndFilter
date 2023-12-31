﻿// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Runtime.Serialization;

namespace PipeFilterCore
{
    ///<inheritdoc cref="Exception"/>
    /// <summary>
    /// Represents a exception for PipeAndFilter.
    /// </summary>
    [Serializable]
    public class PipeAndFilterException : Exception
    {
        /// <summary>
        /// The default status for initialize.
        /// </summary>
        public static readonly PipeStatus StatusInit = new(HandlerType.Pipe,
            HandlerStatus.Created,
            TimeSpan.Zero, null, null, false,null);

        /// <summary>
        /// The status of step (pipe, condition or task).
        /// </summary>
        public PipeStatus? Status { get; }

        private PipeAndFilterException()
        {
        }

        /// <summary>
        /// Create PipeAndFilter-Exception.
        /// </summary>
        ///<param name="status">The status of step (pipe, condition or task).</param>
        ///<param name="message">The message that describes the error.</param> 
        public PipeAndFilterException(PipeStatus status, string? message) : base(message)
        {
            Status = status;
        }

        /// <summary>
        /// Create PipeAndFilter-Exception with innerException.
        /// </summary>
        ///<param name="status">The status of step (pipe, condition or task).</param>
        ///<param name="message">The message that describes the error.</param> 
        ///<param name="innerException">The exception that is the cause of the current exception, or a null reference.</param> 
        public PipeAndFilterException(PipeStatus status, string? message, Exception? innerException) : base(message, innerException)
        {
            Status = status;
        }

        ///<inheritdoc cref="Exception"/>
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")] // add this attribute to the serialization ctor
#endif
        protected PipeAndFilterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        ///<inheritdoc cref="Exception"/>
#if NET8_0_OR_GREATER
    [Obsolete(DiagnosticId = "SYSLIB0051")] // add this attribute to GetObjectData
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            /* your dehydration here */
            base.GetObjectData(info, context);
        }
    }
}
