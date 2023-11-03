// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;
using System.Text.Json;

namespace PipeAndFilter
{
    /// <summary>
    /// Represents a pipe/task event with parameters, values ​​and commands
    /// </summary>
    /// <typeparam name="T">Type of contract</typeparam>
    public class EventPipe<T> where T : class
    {
        private readonly Action<Action<T?>> _changecontract;

        /// <summary>
        /// Create EventPipe
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error</exception>
        private EventPipe()
        {
            throw new PipeAndFilterException("Invalid ctor EventPipe");
        }

        /// <summary>
        /// Create instance of EventPipe (Only internal use or Unit-Test)
        /// </summary>
        /// <param name="changecontract">Handle of changecontract control</param>
        /// <param name="savedpipes">The values saved by pipe</param>
        /// <param name="savedtasks">The values saved by tasks</param>
        /// <param name="fromId">The came from Id</param>
        /// <param name="currentId">The current Id</param>
        /// <param name="fromAlias">The alias came from Id</param>
        /// <param name="currentAlias">The alias current Id</param>
        public EventPipe(Action<Action<T>> changecontract, ImmutableArray<(string? Alias, string Id,string? Result)> savedpipes, ImmutableArray<(string? Alias, string Id, string? Result)> savedtasks, string? fromId, string currentId, string? fromAlias, string? currentAlias)
        {
            _changecontract = changecontract;
            SavedPipes = savedpipes;
            SavedTasks = savedtasks;
            FromAlias = fromAlias;
            FromId = fromId;
            CurrentAlias = currentAlias;
            CurrentId = currentId;
        }

        /// <summary>
        /// The values saved ​​associated with pipes
        /// </summary>
        /// <remarks>
        /// The values ​​are serialized in json
        /// <br>Null result may exist</br>
        /// </remarks>
        public ImmutableArray<(string? Alias, string Id, string? Result)> SavedPipes { get; }

        /// <summary>
        /// The values saved ​​associated with tasks
        /// <remarks>
        /// Data only exists when executed by an aggregator pipe
        /// <br>The values ​​are serialized in json</br>
        /// <br>Null result may exist</br>
        /// </remarks>
        public ImmutableArray<(string? Alias, string Id, string? Result)> SavedTasks { get; }

        /// <summary>
        /// The current Id
        /// </summary>
        public string CurrentId { get; }

        /// <summary>
        /// The previous Id
        /// </summary>
        public string? FromId { get; }

        /// <summary>
        /// The current Alias
        /// </summary>
        public string? CurrentAlias { get; }


        /// <summary>
        /// The previous Alias
        /// </summary>
        public string? FromAlias { get; }

 
        /// <summary>
        /// End EndPipeline control
        /// </summary>
        public void EndPipeline()
        {
            FinishedPipeLine = true;
        }

        /// <summary>
        /// Change value contract
        /// </summary>
        /// <param name="action">The action to change value</param>
        /// <remarks>
        /// The action will only be executed if the contract exists.
        /// <br><see cref="IPipelineInit{T}.Init(T)"/></br>
        /// </remarks>
        public void ChangeContract(Action<T> action)
        {
            _changecontract(action!);
        }

        /// <summary>
        /// Save/overwrite a value associated with this pipe or task 
        /// </summary>
        /// <typeparam name="T1">Type value to save</typeparam>
        /// <param name="value">The value to save</param>
        /// <remarks>
        /// The values ​​will serialize into json
        /// </remarks>
        public void SaveValue<T1>(T1  value)
        {
            IsSaved = true;
            ToRemove = false;
            ValueToSave = JsonSerializer.Serialize<T1>(value);
        }

        /// <summary>
        /// Remove a value associated with this pipe or task 
        /// </summary>
        public void RemoveSavedValue()
        {
            IsSaved = false;
            ToRemove = true;
        }

        internal bool FinishedPipeLine { get; set; }
        internal bool ToRemove { get; set; }
        internal bool IsSaved { get; set; }
        internal string? ValueToSave { get; set; }
    }
}