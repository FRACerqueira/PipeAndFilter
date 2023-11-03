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
        /// Create Pipe Status
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
        /// Create instance (Only internal use or Unit-Test)
        /// </summary>
        /// <param name="typeExec"><see cref="HandlerType"/></param>
        /// <param name="value"><see cref="TaskStatus"/></param>
        /// <param name="elapsedtime"><see cref="TimeSpan"/></param>
        /// <param name="alias">The execution alias</param>
        /// <param name="gotoAlias">The Alias ​​of go to</param>
        /// <param name="condition">if the result of the condition is true</param>
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
        /// The running status
        /// </summary>
        public TaskStatus Value { get; }

        /// <summary>
        /// The elapsed time
        /// </summary>
        public TimeSpan Elapsedtime { get; }

        /// <summary>
        /// The Handler Type
        /// </summary>
        public HandlerType TypeExec { get; }

        /// <summary>
        /// The execution alias
        /// </summary>
        public string? Alias { get; }

        /// <summary>
        /// The Alias ​​of go to
        /// </summary>
        public string? GotoAlias { get; }

        /// <summary>
        /// The condition result
        /// </summary>
        public bool Condition { get; }

    }
}