// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the result of PipeAndFilter.
    /// </summary>
    public readonly struct ResultPipeAndFilter<T> where T : class
    {
        /// <summary>
        /// Create Result of PipeAndFilter.
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error.</exception>
        public ResultPipeAndFilter()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit,
                "Invalid ctor ResultPipeAndFilter");
        }

        /// <summary>
        /// Create Result of PipeAndFilter (Only internal use or Unit-Test).
        /// </summary>
        /// <param name="value">The contract value.</param>
        /// <param name="aborted">If aborted.</param>
        /// <param name="exception">The exception, if any.</param>
        /// <param name="status">Detailed running status of all pipes.</param>
        public ResultPipeAndFilter(T? value, bool aborted, PipeAndFilterException? exception, ImmutableArray<PipeRanStatus> status)
        {
            Value = value;
            Aborted = aborted;
            Exception = exception;
            Status = status;
        }

        /// <summary>
        /// The Contract value.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// If aborted.
        /// </summary>
        public bool Aborted { get; }

        /// <summary>
        /// The exception (pipe,condition or task), if any.
        /// </summary>
        public PipeAndFilterException? Exception { get; }

        /// <summary>
        /// The status details of all pipes.
        /// </summary>
        public ImmutableArray<PipeRanStatus> Status { get; }

    }
}