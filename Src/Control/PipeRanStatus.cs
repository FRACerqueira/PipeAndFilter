// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents the ran status of the pipe
    /// </summary>
    public readonly struct PipeRanStatus
    {
        /// <summary>
        /// Create PipeRanStatus
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error</exception>
        public PipeRanStatus()
        {
            throw new PipeAndFilterException("Invalid ctor PipeRanStatus");
        }

        /// <summary>
        /// Create PipeRanStatus
        /// </summary>
        /// <param name="id">The id pipe</param>
        /// <param name="alias">The alias pipe</param>
        /// <param name="details">The detailed status of all runs</param>
        public PipeRanStatus(string id, string? alias, IEnumerable<PipeStatus> details)
        {
            Id = id;
            Alias = alias;
            StatusDetails = details;
        }

        /// <summary>
        /// Get alias pipe
        /// </summary>
        public string? Alias { get; }

        /// <summary>
        /// Get last pipe status handle run
        /// </summary>
        public PipeStatus Status
        {
            get
            {
                if (StatusDetails.Any(x => x.TypeExec == HandlerType.Pipe))
                {
                    return StatusDetails.Where(x => x.TypeExec == HandlerType.Pipe).Last();
                }
                return new PipeStatus(HandlerType.Pipe,
                    TaskStatus.WaitingForActivation,
                    TimeSpan.Zero,
                    null,
                    null, false);
            }
        }

        /// <summary>
        /// Get number of times the pipe was executed
        /// </summary>
        public readonly int Count => StatusDetails.Count(x => x.TypeExec == HandlerType.Pipe);

        /// <summary>
        /// Get detailed status for each step(pipe,conditions and tasks)
        /// </summary>
        public IEnumerable<PipeStatus> StatusDetails { get; }

        /// <summary>
        /// Get pipe Id
        /// </summary>
        public string Id { get; }
    }
}
