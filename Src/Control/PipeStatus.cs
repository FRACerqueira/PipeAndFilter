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
        /// <br>Do not use this constructor!</br>
        /// </summary>
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
        /// <param name="value">The Status. See <see cref="HandlerStatus"/>.</param>
        /// <param name="elapsedtime">The elapsed time. See <see cref="TimeSpan"/>.</param>
        /// <param name="alias">The alias execution.</param>
        /// <param name="gotoAlias">The alias link.</param>.
        /// <param name="condition">The result of the condition for execution.</param>
        /// <param name="toAliasCondition">The alias to result of the condition.</param>
        public PipeStatus(HandlerType typeExec, HandlerStatus value, TimeSpan elapsedtime, string? alias, string? gotoAlias, bool condition, string? toAliasCondition)
        {
            ToAliasCondition = toAliasCondition;
            TypeExec = typeExec;
            Value = value;
            Elapsedtime = elapsedtime;
            Alias = alias;
            GotoAlias = gotoAlias;
            Condition = condition;
            DateRef = DateTime.Now.ToUniversalTime();
        }

        /// <summary>
        /// The running status.
        /// </summary>
        public HandlerStatus Value { get; }

        /// <summary>
        /// The elapsed time.
        /// </summary>
        public TimeSpan Elapsedtime { get; }

        /// <summary>
        /// The UTC Date reference.
        /// </summary>
        public DateTime DateRef { get; }

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

        /// <summary>
        /// The alias to result of the condition.
        /// </summary>
        public string? ToAliasCondition { get; }

    }
}