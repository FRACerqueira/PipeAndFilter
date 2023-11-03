// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;

namespace PipeAndFilter
{
    /// <summary>
    /// Represents the result of pipeline
    /// </summary>
    public readonly struct ResultPipeline<T> where T : class
    {
        /// <summary>
        /// Create ResultPipeline
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error</exception>
        public ResultPipeline()
        {
            throw new PipeAndFilterException("Invalid ctor ResultPipeline");
        }

        /// <summary>
        /// Create ResultPipeline
        /// </summary>
        /// <param name="value">The contract value</param>
        /// <param name="aborted">Pipeline aborted</param>
        /// <param name="exception">The last exception</param>
        /// <param name="status">Detailed running status of all pipes</param>
        public ResultPipeline(T? value, bool aborted, Exception? exception, ImmutableArray<PipeRanStatus> status)
        {
            Value = value;
            Aborted = aborted;
            PipeException = exception;
            Status = status;
        }

        /// <summary>
        /// Contract contract pipeline 
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Pipeline is aborted.
        /// </summary>
        public bool Aborted { get; }

        /// <summary>
        /// The exception in condition or command pipe.
        /// </summary>
        public Exception? PipeException { get; }

        /// <summary>
        /// StatusDetails of all pipes
        /// </summary>
        public ImmutableArray<PipeRanStatus> Status { get; }

    }
}