﻿// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the ran status of the pipe.
    /// </summary>
    public readonly struct PipeRanStatus
    {
        /// <summary>
        /// Create PipeRanStatus.
        /// <br>Do not use this constructor!</br>
        /// </summary>
        /// <exception cref="PipeAndFilterException">Message error</exception>
        public PipeRanStatus()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit, 
                "Invalid ctor PipeRanStatus");
        }

        /// <summary>
        /// Create PipeRanStatus (Only internal use or Unit-Test).
        /// </summary>
        /// <param name="id">The pipe Id.</param>
        /// <param name="alias">The pipe alias.</param>
        /// <param name="details">The detailed status of all runs.</param>
        public PipeRanStatus(string id, string? alias, ImmutableArray<PipeStatus> details)
        {
            Id = id;
            Alias = alias;
            StatusDetails = details;
        }

        /// <summary>
        /// The pipe alias.
        /// </summary>
        public string? Alias { get; }

        /// <summary>
        /// The last execution status of the pipe.
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
                    HandlerStatus.Created,
                    TimeSpan.Zero,
                    null,
                    null, false, null);
            }
        }

        /// <summary>
        /// The number of times the pipe has been executed.
        /// </summary>
        public readonly int Count => StatusDetails.Count(x => x.TypeExec == HandlerType.Pipe);

        /// <summary>
        /// The detailed status of each step (pipe, conditions and tasks).
        /// </summary>
        public ImmutableArray<PipeStatus> StatusDetails { get; }

        internal string Id { get; }
    }
}
