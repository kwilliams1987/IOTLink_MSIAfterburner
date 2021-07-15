using System.Collections.Generic;
using static IOTLinkAddon.Shared.Icons;

namespace IOTLinkAddon.Shared
{
    public class MonitorItem
    {
        public SourceType SourceId { get; private set; }
        public string Topic { get; private set; }
        public string Label { get; private set; }
        public string Icon { get; private set; }
        public string ConfigKey { get; private set; }
        public string Prefix { get; private set; }
        public int Precision { get;private set; }

        public MonitorItem(SourceType sourceId, string topic, string label, string icon, string configKey, string prefix, int precision = 0)
        {
            SourceId = sourceId;
            Topic = topic;
            Label = label;
            Icon = icon;
            ConfigKey = configKey;
            Prefix = prefix;
            Precision = precision;
        }

        public override string ToString() => $"{SourceId}: {Label}";

        public static readonly IEnumerable<MonitorItem> MonitorItems = new List<MonitorItem>()
        {
            new MonitorItem(SourceType.Framerate, "Framerate/FPS", "Framerate", Monitor, "Framerate", "FPS"),
            new MonitorItem(SourceType.FramerateAverage, "Framerate/Average", "Average", Monitor, "Framerate", "FPS"),
            new MonitorItem(SourceType.Framerate1Percent, "Framerate/1Percent", "1% Low", Monitor, "Framerate", "FPS"),

            new MonitorItem(SourceType.CpuTemp, "CPU/Temperature", "CPU Temperature", Thermometer, "CPU:Temp", "CPU"),
            new MonitorItem(SourceType.CpuUsage, "CPU/Usage", "CPU Usage", Processor, "CPU:Usage", "CPU"),
            new MonitorItem(SourceType.CpuClock, "CPU/Clock", "CPU Clock Speed", Processor, "CPU:Clock", "CPU"),
            new MonitorItem(SourceType.CpuPower, "CPU/Power", "CPU Power", LightningBolt, "CPU:Power", "CPU"),

            new MonitorItem(SourceType.CommitCharge, "Memory/CommitCharge", "Commit Charge", Memory, "Memory:CommitCharge", "Memory"),
            new MonitorItem(SourceType.MemoryUsage, "Memory/Used", "Memory Used", Memory, "Memory:Usage", "Memory"),

            new MonitorItem(SourceType.GpuCoreClock, "GPU{0}/Clock", "Core Clock", ExpansionCard, "GPU:Clock", "GPU{0}"),
            new MonitorItem(SourceType.GpuPower, "GPU{0}/Power", "Power", PowerPlug, "GPU:Power", "GPU{0}"),
            new MonitorItem(SourceType.GpuShaderClock, "GPU{0}/ShaderClock", "Shader Clock", Drawing, "GPU:ShaderClock", "GPU{0}"),
            new MonitorItem(SourceType.GpuCoreTemp, "GPU{0}/Temperature", "Core Temperature", Thermometer, "GPU:Temp", "GPU{0}"),
            new MonitorItem(SourceType.GpuTempLimit, "GPU{0}/TemperatureLimit", "Temperature Limit", Thermometer, "GPU:Temp", "GPU{0}"),
            new MonitorItem(SourceType.GpuUsage, "GPU{0}/CoreUsage", "Core Usage", Speedometer, "GPU:Usage", "GPU{0}"),

            new MonitorItem(SourceType.GpuCoreVoltage, "GPU{0}/PowerVoltage", "Core Voltage", LightningBolt, "GPU:Voltage", "GPU{0}", 3),
            new MonitorItem(SourceType.GpuPowerUsage, "GPU{0}/PowerUsage", "Power Usage", LightningBolt, "GPU:Power", "GPU{0}", 1),
            new MonitorItem(SourceType.GpuPowerLimit, "GPU{0}/PowerLimit", "Power Limit", LightningBolt, "GPU:Power", "GPU{0}"),

            new MonitorItem(SourceType.GpuFan1Speed, "GPU{0}/Fan1Speed", "Fan 1 Speed", Fan1, "GPU:FanSpeed", "GPU{0}"),
            new MonitorItem(SourceType.GpuFan2Speed, "GPU{0}/Fan2Speed", "Fan 2 Speed", Fan2, "GPU:FanSpeed", "GPU{0}"),
            new MonitorItem(SourceType.GpuFan3Speed, "GPU{0}/Fan3Speed", "Fan 3 Speed", Fan3, "GPU:FanSpeed", "GPU{0}"),
            new MonitorItem(SourceType.GpuFan1Rpm, "GPU{0}/Fan1RPM", "Fan 1 RPM", Fan1, "GPU:FanSpeed", "GPU{0}"),
            new MonitorItem(SourceType.GpuFan2Rpm, "GPU{0}/Fan2RPM", "Fan 2 RPM", Fan2, "GPU:FanSpeed", "GPU{0}"),
            new MonitorItem(SourceType.GpuFan3Rpm, "GPU{0}/Fan3RPM", "Fan 3 RPM", Fan3, "GPU:FanSpeed", "GPU{0}"),

            new MonitorItem(SourceType.GpuMemoryClock, "GPU{0}/MemoryClock", "Memory Clock", Memory, "GPU:MemoryClock", "GPU{0}"),
            new MonitorItem(SourceType.GpuMemoryTemp, "GPU{0}/MemoryTemperature", "Memory Temperature", Thermometer, "GPU:MemoryTemp", "GPU{0}"),
            new MonitorItem(SourceType.GpuMemoryUsed, "GPU{0}/MemoryUsed", "Memory Used", Memory, "GPU:MemoryUsed", "GPU{0}"),
            new MonitorItem(SourceType.GpuMemoryVoltage, "GPU{0}/MemoryVoltage", "Memory Voltage", LightningBolt, "GPU:MemoryVoltage", "GPU{0}"),
            new MonitorItem(SourceType.GpuFbUsage, "GPU{0}/FrameBuffer", "Frame Buffer Usage", Buffer, "GPU:FrameBufferUsage", "GPU{0}"),
            new MonitorItem(SourceType.GpuVidUsage, "GPU{0}/VIDUsage", "Video Engine Load", Video, "GPU:VIDUsage", "GPU{0}"),
            new MonitorItem(SourceType.GpuBusUsage, "GPU{0}/BusUsage", "Bus Usage", ExpansionCard, "GPU:BusUsage", "GPU{0}"),

            new MonitorItem(SourceType.GpuVrmTemp, "GPU{0}/VRMTemperature", "VRM Temperature", Thermometer, "GPU:VRMTemp", "GPU{0}"),
        };
    }
}
