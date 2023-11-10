using System.Collections.Immutable;

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

        public ImmutableDictionary<string, string> AliasToId => _parameters.AliasToId;

        public ImmutableDictionary<string, string?> IdToAlias => _parameters.IdToAlias;

        public ImmutableList<PipeCommand<T>> Pipes => _parameters.Pipes;


        #endregion

        #region IPipeAndFilterServiceBuild

        IPipeAndFilterInit<T> IPipeAndFilterService<T>.Create()
        {
            return new PipeAndFilterControl<T>(this);
        }

        #endregion
    }
}
