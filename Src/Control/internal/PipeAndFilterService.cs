using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipeFilterCore.CommandsInterface;

namespace PipeFilterCore.Control
{
    internal class PipeAndFilterService<T> : IPipeAndFilterOptions<T>, IPipeAndFilterServiceBuild<T> where T : class
    {
        private readonly string? _serviceid;
        readonly PipeAndFilterBuild<T> _parameters;

        public PipeAndFilterService(string? serviceid, PipeAndFilterBuild<T> parameters)
        {
            _serviceid = serviceid;
            _parameters = parameters;
        }

        public string? ServiceId => _serviceid;

        public Dictionary<string, string> AliasToId => _parameters._aliasToId;

        public Dictionary<string, string?> IdToAlias => _parameters._idToAlias;

        public Dictionary<string, int> MaxDegreeProcess => _parameters._maxDegreeProcess;

        public Dictionary<string, (Func<EventPipe<T>, CancellationToken, 
            Task> pipehandle, 
            bool aggregateTasks, 
            List<PipeCondition<T>> precondhandle, 
            List<PipeStatus> status, 
            List<(string Id, Func<EventPipe<T>, CancellationToken, Task> TaskHandle, PipeCondition<T>? TaskCondition, string? NameTask, string? NameCondition)> tasks)> 
            Pipes => _parameters._pipes;

        IPipeAndFilterRunService<T> IPipeAndFilterServiceBuild<T>.Create()
        {
            return new PipeAndFilterControl<T>(this);
        }
    }
}
