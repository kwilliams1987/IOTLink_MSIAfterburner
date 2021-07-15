namespace IOTLinkAddon.Shared.MemoryManager
{
    public class AfterburnerMetric
    {
        public uint? GpuIndex { get; set; }
        public string Units { get; set; }
        public float? Value { get; set; }
        public float Minimum { get; set; }
        public float Maximum { get; set; }

        public MonitorItem ItemType { get; set; }
    }
}
