// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;
using System.Text.Json;

namespace PipeAndFilter
{
    /// <summary>
    /// Represents a EventPipe 
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
        /// Get values saved ​​associated with pipes
        /// <br>The values ​​are serialized in json</br>
        /// <br>Null result may exist</br>
        /// </summary>
        public ImmutableArray<(string? Alias, string Id, string? Result)> SavedPipes { get; }

        /// <summary>
        /// Get values saved ​​associated with tasks
        /// <br>The values ​​are serialized in json</br>
        /// <br>Null result may exist</br>
        /// </summary>
        public ImmutableArray<(string? Alias, string Id, string? Result)> SavedTasks { get; }

        /// <summary>
        /// Get current Id
        /// </summary>
        public string CurrentId { get; }

        /// <summary>
        /// Get from Id
        /// </summary>
        public string? FromId { get; }

        /// <summary>
        /// Get current Alias
        /// </summary>
        public string? CurrentAlias { get; }


        /// <summary>
        /// Get from Alias
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
        /// <param name="action"></param>
        public void ChangeContract(Action<T> action)
        {
            _changecontract(action!);
        }

        /// <summary>
        /// Save/overwrite a value associated with this pipe or task 
        /// <br>The values ​​will serialize into json</br>
        /// </summary>
        /// <typeparam name="T1">Type value to save</typeparam>
        /// <param name="value">The value to save</param>
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