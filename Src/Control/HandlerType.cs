// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterPlus
{
    /// <summary>
    /// Represents a handler type.
    /// </summary>
    public enum HandlerType
    {
        /// <summary>
        /// No Type, Runtime Initialize
        /// </summary>
        None,
        /// <summary>
        /// Type condition.
        /// </summary>
        Condition,
        /// <summary>
        /// Type condition with link to another pipe.
        /// </summary>
        ConditionGoto,
        /// <summary>
        /// Type pipe.
        /// </summary>
        Pipe,
        /// <summary>
        /// Type task.
        /// </summary>
        Task,
        /// <summary>
        /// Type aggregate task.
        /// </summary>
        AggregateTask,
        /// <summary>
        /// Type condition task.
        /// </summary>
        ConditionTask,
    }
}
