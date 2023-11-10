// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

namespace PipeFilterCore
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
        /// Type aggregate pipe.
        /// </summary>
        AggregatePipe,
        /// <summary>
        /// Type condition task.
        /// </summary>
        ConditionTask,
        /// <summary>
        /// Type after condition.
        /// </summary>
        AfterCondition,
        /// <summary>
        /// Type after condition with link to another pipe.
        /// </summary>
        AfterConditionGoto,
        /// <summary>
        /// Type after pipe.
        /// </summary>
        AfterPipe,
        /// <summary>
        /// Type after task.
        /// </summary>
        AfterTask,
        /// <summary>
        /// Type after aggregate task.
        /// </summary>
        AfterAggregatePipe,
        /// <summary>
        /// Type after condition task.
        /// </summary>
        AfterConditionTask,

    }
}
