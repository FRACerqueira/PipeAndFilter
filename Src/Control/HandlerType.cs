// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeAndFilter
{
    /// <summary>
    /// Represents a handler type
    /// </summary>
    public enum HandlerType
    {
        /// <summary>
        /// Execution condition handler
        /// </summary>
        Condition,
        /// <summary>
        /// Execution condition with goto handler
        /// </summary>
        ConditionGoto,
        /// <summary>
        /// Execution pipe handler
        /// </summary>
        Pipe,
        /// <summary>
        /// Execution task handler
        /// </summary>
        Task,
        /// <summary>
        /// Execution aggregate task handler
        /// </summary>
        AggregateTask,
        /// <summary>
        /// Execution condition task handler
        /// </summary>
        ConditionTask,
    }
}
