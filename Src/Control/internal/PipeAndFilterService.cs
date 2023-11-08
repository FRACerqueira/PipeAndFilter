using System.Collections.Immutable;
using System.Threading.Tasks;

namespace PipeFilterCore
{
    internal class PipeAndFilterService<T> : IPipeAndFilterOptions<T>, IPipeAndFilterService<T> where T : class
    {
        private readonly string? _serviceid;
        private readonly IPipeAndFilterOptions<T> _parameters;

        public PipeAndFilterService(string? serviceid, IPipeAndFilterOptions<T> parameters)
        {
            _serviceid = serviceid;
            _parameters = parameters;
        }

        #region IPipeAndFilterOptions

        public string? ServiceId => _serviceid;

        public IImmutableDictionary<string, string> AliasToId => _parameters.AliasToId;

        public IImmutableDictionary<string, string?> IdToAlias => _parameters.IdToAlias;

        public IImmutableDictionary<string, int> MaxDegreeProcess => _parameters.MaxDegreeProcess;

        public IImmutableList<PipeCommand<T>> Pipes => _parameters.Pipes;

        public IImmutableDictionary<string, bool> AggregateTasks => _parameters.AggregateTasks;

        public IImmutableDictionary<string, IImmutableList<PipeCondition<T>>> PreConditions => _parameters.PreConditions;
   
        public IImmutableDictionary<string, IImmutableList<PipeTask<T>>> Tasks => _parameters.Tasks;

        #endregion

        #region IPipeAndFilterServiceBuild

        IPipeAndFilterInit<T> IPipeAndFilterService<T>.Create()
        {
            return new PipeAndFilterControl<T>(this);
        }

        #endregion
    }
}
