// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents the status of step (pipe, condition or task)
    /// </summary>
    public readonly struct PipeStatus
    {
        /// <summary>
        /// Create PipeStatus
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error</exception>
        public PipeStatus()
        {
            throw new PipeAndFilterException("Invalid ctor PipeStatus");
        }

        /// <summary>
        /// Create instance
        /// </summary>
        /// <param name="typeExec"><see cref="HandlerType"/></param>
        /// <param name="value"><see cref="TaskStatus"/></param>
        /// <param name="elapsedtime"><see cref="TimeSpan"/></param>
        /// <param name="alias">The alias</param>
        /// <param name="gotoAlias">the go to alias</param>
        /// <param name="condition">if Status is condition handle</param>
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
        /// Get status execution
        /// </summary>
        public TaskStatus Value { get; }

        /// <summary>
        /// Get the elapsed time
        /// </summary>
        public TimeSpan Elapsedtime { get; }

        /// <summary>
        /// Get Type execution
        /// </summary>
        public HandlerType TypeExec { get; }

        /// <summary>
        /// Get execution alias
        /// </summary>
        public string? Alias { get; }

        /// <summary>
        /// Get go to Alias
        /// </summary>
        public string? GotoAlias { get; }

        /// <summary>
        /// Get condition result
        /// </summary>
        public bool Condition { get; }

    }
}