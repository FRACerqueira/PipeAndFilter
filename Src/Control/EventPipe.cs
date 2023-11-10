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
        private readonly List<(string id,string? value, bool toremove)> _toSaveRemove = new();
        private readonly ImmutableDictionary<string, string?> _savedvalues;

        /// <summary>
        /// Create Event-Pipe.
        /// <br>Do not use this constructor!</br>
        /// </summary>
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
        /// <param name="savedvalues">The values saved.</param>
        /// <param name="fromId">The previous Id.</param>
        /// <param name="currentId">The current Id.</param>
        /// <param name="fromAlias">The previous alias.</param>
        /// <param name="currentAlias">The current alias.</param>
        public EventPipe(string? cid, ILogger logger, Action<Action<T?>> changecontract, ImmutableDictionary<string, string?> savedvalues, string? fromId, string currentId, string? fromAlias, string? currentAlias)
        {
            _changecontract = changecontract;
            CorrelationId = cid;
            Logger = logger;
            _savedvalues = savedvalues;
            FromAlias = fromAlias;
            FromId = fromId;
            CurrentAlias = currentAlias;
            CurrentId = currentId;
        }

        /// <summary>
        /// Try get value saved ​​associated with a unique key.
        /// <br>The values ​​are serialized in json.</br> 
        /// <br>Null result may exist.</br>
        /// </summary>
        /// <param name="id">
        /// The unique key Id.
        /// </param>
        /// <param name="value">
        /// The value saved if any.
        /// </param>
        /// <returns>True if value exists, otherwise false with null value</returns>
        public bool TrySavedValue(string id, out string? value)
        {
            if (id == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "id cannot be null");
            }
            return _savedvalues.TryGetValue(id, out value); 
        }

        /// <summary>
        /// The current Alias.
        /// </summary>
        public string? CurrentAlias { get; }


        /// <summary>
        /// The log handler.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// The Correlation Id.
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
        /// Thread Safe Access of contract.
        /// <br>The action will only be executed if the contract exists(not null).</br>
        /// </summary>
        /// <param name="action">
        /// The action to access.
        /// <br>The action will only be executed if the contract exists(not null).</br>
        /// </param>
        public void ThreadSafeAccess(Action<T> action)
        {
            _changecontract(action!);
        }

        /// <summary>
        /// Save/replace a value associated with a unique key at the end of this event(If this event is not a task event).
        /// <br>Values ​​saved in the task event will only take effect in the pipe aggregation event</br>
        /// <br>A task event cannot see values ​​saved and/or removed by another task.</br>
        /// <br>In a task event, Never try to overwrite a value already saved by another event, the results may not be as expected as the execution sequence is not guaranteed.</br>
        /// <br>The values ​​will serialize into json.</br>
        /// </summary>
        /// <typeparam name="T1">Type value to save.</typeparam>
        /// <param name="id">
        /// The unique key Id.
        /// </param>
        /// <param name="value">
        /// The value to save.
        /// </param>
        public void SaveValueAtEnd<T1>(string id, T1  value)
        {
            if (id == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "id cannot be null");
            }
            _toSaveRemove.Add((id, JsonSerializer.Serialize(value), false));
        }

        /// <summary>
        /// Remove a value associated with a unique key at the end of this event (If this event is not a task event).
        /// <br>Values ​​removed in the task event will only take effect in the pipe aggregation event</br>
        /// <br>A task event cannot see values ​​saved and/or removed by another task.</br>
        /// <br>In a task event, Never try to overwrite a value already saved by another event, the results may not be as expected as the execution sequence is not guaranteed.</br>
        /// </summary>
        /// <param name="id">
        /// The unique key Id.
        /// </param>
        public void RemoValueAtEnd(string id)
        {
            if (id == null)
            {
                throw new PipeAndFilterException(
                    PipeAndFilterException.StatusInit,
                    "id cannot be null");
            }
            _toSaveRemove.Add((id, null, true));
        }

        internal string CurrentId { get; }
        internal string? FromId { get; }
        internal bool FinishedPipeAndFilter { get; set; }
        internal IEnumerable<(string id, string? value, bool toremove)> ToSaveRemove => _toSaveRemove;
    }
}