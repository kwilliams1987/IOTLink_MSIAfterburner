using System.Collections.Generic;

namespace IOTLinkAddon.Shared.Interop
{
    struct Gpu
    {
        public int Index;
        public string GpuId;
        public uint Memory;
        public string Driver;
        public string Device;
        public string Family;
        public string BIOS;

        public List<Metric> Metrics;
    }
}
