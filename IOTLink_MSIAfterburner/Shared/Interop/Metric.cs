namespace IOTLinkAddon.Shared.Interop
{
    struct Metric
    {
        public SourceType SourceId;
        public string Units;
        public float? Value;
        public float Minimum;
        public float Maximum;

        public override string ToString() => $"{SourceId}: {Value} {Units}";
    }
}
