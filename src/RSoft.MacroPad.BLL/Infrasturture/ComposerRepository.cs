
using System.Collections.Generic;
using System.Linq;
using RSoft.MacroPad.BLL.Infrasturture.Model;
using RSoft.MacroPad.BLL.Infrasturture.Protocol;
using RSoft.MacroPad.BLL.Infrasturture.Protocol.WebHub;

namespace RSoft.MacroPad.BLL.Infrasturture
{
    public class ComposerRepository
    {
        public List<(IReportComposer Composer, ProtocolType Type, byte Version)> _cache;

        public ComposerRepository()
        {
            _cache = new List<(IReportComposer Composer, ProtocolType Type, byte Version)>();
        }

        public IReportComposer Get(ProtocolType type, byte version)
        {
            var i = _cache.FindIndex(x => x.Type == type && x.Version == version);
            
            if (i != -1)
                return _cache[i].Composer;

            IReportComposer result;
            switch (type)
            {
                case ProtocolType.Legacy: result = new LegacyReportComposer(version); break;
                case ProtocolType.WebHub: result = new WebHubReportComposer(); break;
                default: result = new ExtendedReportComposer(version); break;
            }
            _cache.Add((result, type, version));
            return result;
        }

    }
}
