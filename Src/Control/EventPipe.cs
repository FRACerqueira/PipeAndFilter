// ********************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PipeAndFilter project under MIT license
// ********************************************************************************************

using System.Collections.Immutable;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace PipeFilterCore
{
    /// <summary>
    /// Represents a pipe/task event with parameters, values ​​and commands.
    /// </summary>
    /// <typeparam name="T">Type of contract.</typeparam>
    public class EventPipe<T> where T : class
    {
        private readonly Action<Action<T?>> _changecontract;

        /// <summary>
        /// Create Event-Pipe.
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        /// <exception cref="PipeAndFilterException">Message error.</exception>
        private EventPipe()
        {
            throw new PipeAndFilterException(
                PipeAndFilterException.StatusInit,
                "Invalid ctor EventPipe");
        }

        /// <summary>
        /// Create instance of Event-Pipe (Only internal use or Unit-Test).
        /// </summary>
        /// <param name="cid">The correlation Id.</param>
        /// <param name="logger">Handle of log.</param>
        /// <param name="changecontract">Handle of changecontract.</param>
        /// <param name="savedpipes">The values saved by pipe.</param>
        /// <param name="savedtasks">The values saved by tasks.</param>
        /// <param name="fromId">The previous Id.</param>
        /// <param name="currentId">The current Id.</param>
        /// <param name="fromAlias">The previous alias.</param>
        /// <param name="currentAlias">The current alias.</param>
        public EventPipe(string? cid, ILogger logger, Action<Action<T?>> changecontract, ImmutableArray<(string? Alias, string Id,string? Result)> savedpipes, ImmutableArray<(string? Alias, string Id, string? Result)> savedtasks, string? fromId, string currentId, string? fromAlias, string? currentAlias)
        {
            _changecontract = changecontract;
            CorrelationId = cid;
            Logger = logger; 
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
        /// The values ​​are serialized in json.
        /// <br>Null result may exist.</br>
        /// </remarks>.
        public ImmutableArray<(string? Alias, string Id, string? Result)> SavedPipes { get; }

        /// <summary>
        /// The values saved ​​associated with tasks.
        /// </summary>
        /// <remarks>
        /// Data only exists when executed by an aggregator pipe.
        /// <br>The values ​​are serialized in json.</br>
        /// <br>Null result may exist.</br>
        /// </remarks>
        public ImmutableArray<(string? Alias, string Id, string? Result)> SavedTasks { get; }

        /// <summary>
        /// The current Alias.
        /// </summary>
        public string? CurrentAlias { get; }


        /// <summary>
        /// The log handler
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// The Correlation Id
        /// </summary>
        public string? CorrelationId { get; }

        /// <summary>
        /// The previous Alias.
        /// </summary>
        public string? FromAlias { get; }


        /// <summary>
        /// End PipeAndFilter.
        /// </summary>
        public void EndPipeAndFilter()
        {
            FinishedPipeAndFilter = true;
        }

        /// <summary>
        /// Change value contract.
        /// </summary>
        /// <param name="action">The action to change value.</param>
        /// <remarks>
        /// The action will only be executed if the contract exists.
        /// <br>See <see cref="IPipeAndFilterInit{T}.Init(T)"/>.</br>
        /// </remarks>
        public void ChangeContract(Action<T> action)
        {
            _changecontract(action!);
        }

        /// <summary>
        /// Save/overwrite a value associated with this pipe or task.
        /// </summary>
        /// <typeparam name="T1">Type value to save.</typeparam>
        /// <param name="value">The value to save.</param>
        /// <remarks>
        /// The values ​​will serialize into json.
        /// </remarks>
        public void SaveValue<T1>(T1  value)
        {
            IsSaved = true;
            ToRemove = false;
            ValueToSave = JsonSerializer.Serialize(value);
        }

        /// <summary>
        /// Remove a value associated with this pipe or task .
        /// </summary>
        public void RemoveSavedValue()
        {
            IsSaved = false;
            ToRemove = true;
        }

        internal string CurrentId { get; }
        internal string? FromId { get; }
        internal bool FinishedPipeAndFilter { get; set; }
        internal bool ToRemove { get; set; }
        internal bool IsSaved { get; set; }
        internal string? ValueToSave { get; set; }
    }
}