namespace Models.Algo
{
    using System.Linq;
    using MultiTerminal.Connections;
    using System.Collections.Generic;

    internal class OptChainLogic
    {
        internal IConnectorLogger _logger;
        private IEnumerable<OptChain> OptChainsCpy { get; }

        public OptChainLogic(IConnectorLogger logger, IEnumerable<OptChain> OptChains)
        {
            _logger = logger;
            this.OptChainsCpy = OptChains;
        }

        public OptChain GetOptChainByStrike(string strike)
        {
            if (!string.IsNullOrEmpty(strike)) // if param doesn't empty
            {
                var found = OptChainsCpy.FirstOrDefault(s => s.Strike == strike);
                if (found != null)
                    return found;
            }
            return null;
        }
    }
}
