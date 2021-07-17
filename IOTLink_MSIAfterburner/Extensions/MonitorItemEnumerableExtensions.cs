using System.Collections.Generic;
using System.Linq;

namespace IOTLinkAddon.Shared
{
    public static class MonitorItemEnumerableExtensions
    {
        public static MonitorItem Find(this IEnumerable<MonitorItem> collection, SourceType sourceId)
            => collection.FirstOrDefault(m => m.SourceId == sourceId);
    }
}
