// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
{
    /// <summary>
    /// Represents the status of step (pipe, condition or task).
    /// </summary>
    public readonly struct PipeStatus
    {
        /// <summary>
        /// Create Pipe Status.
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error.</exception>
        public PipeStatus()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit, 
                "Invalid ctor PipeStatus");
        }

        /// <summary>
        /// Create instance (Only internal use or Unit-Test).
        /// </summary>
        /// <param name="typeExec">Type handle. See <see cref="HandlerType"/>.</param>
        /// <param name="value">The Status. See <see cref="TaskStatus"/>.</param>
        /// <param name="elapsedtime">The elapsed time. See <see cref="TimeSpan"/>.</param>
        /// <param name="alias">The alias execution.</param>
        /// <param name="gotoAlias">The alias link.</param>.
        /// <param name="condition">The result of the condition for execution.</param>
        public PipeStatus(HandlerType typeExec, TaskStatus value, TimeSpan elapsedtime, string? alias, string? gotoAlias, bool condition)
        {
            TypeExec = typeExec;
            Value = value;
            Elapsedtime = elapsedtime;
            Alias = alias;
            GotoAlias = gotoAlias;
            Condition = condition;
        }

        /// <summary>
        /// The running status.
        /// </summary>
        public TaskStatus Value { get; }

        /// <summary>
        /// The elapsed time.
        /// </summary>
        public TimeSpan Elapsedtime { get; }

        /// <summary>
        /// The Type handle.
        /// </summary>
        public HandlerType TypeExec { get; }

        /// <summary>
        /// The alias execution.
        /// </summary>
        public string? Alias { get; }

        /// <summary>
        /// The Alias ​​link.
        /// </summary>
        public string? GotoAlias { get; }

        /// <summary>
        /// The result of the condition. 
        /// </summary>
        public bool Condition { get; }

    }
}