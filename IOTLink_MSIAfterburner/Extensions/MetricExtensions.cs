using IOTLinkAPI.Configs;
using System.Collections.Generic;
using System.Linq;

namespace IOTLinkAddon.Shared.Interop
{
    static class MetricExtensions
    {
        public static MonitorItem GetMonitorItem(this Metric metric) 
            => MonitorItem.MonitorItems.FirstOrDefault(m => m.SourceId == metric.SourceId);

        public static bool IsEnabled(this Metric metric, Configuration configuration)
        {
            var configKey = metric.GetMonitorItem()?.ConfigKey;

            if (string.IsNullOrWhiteSpace(configKey))
            {
                return false;
            }
                
            return configuration.GetValue($"sensors:{configKey}:enabled", false);
        }

        public static IEnumerable<Metric> WhereEnabled(this IEnumerable<Metric> items, Configuration configuration) 
            => items.Where(m => m.IsEnabled(configuration));
    }
}
