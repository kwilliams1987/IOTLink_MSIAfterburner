namespace IOTLinkAddon.Shared
{
    public class NumericMonitorItem : MonitorItem
    {
        public int Precision { get; private set; }

        public NumericMonitorItem(SourceType sourceId, string topic, string label, string icon, string configKey, string prefix, int precision = 0) 
            : base(sourceId, topic, label, icon, configKey, prefix)
        {
            Precision = precision;
        }

        public override string Format(float? value)
            => value?.ToString($"F{Precision}");
    }
}
