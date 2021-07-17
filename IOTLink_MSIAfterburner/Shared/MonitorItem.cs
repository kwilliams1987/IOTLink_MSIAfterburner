using System.Collections.Generic;
using static IOTLinkAddon.Shared.Icons;

namespace IOTLinkAddon.Shared
{
    public abstract class MonitorItem
    {
        public SourceType SourceId { get; private set; }
        public string Topic { get; private set; }
        public string Label { get; private set; }
        public string Icon { get; private set; }
        public string ConfigKey { get; private set; }
        public string Prefix { get; private set; }

        protected MonitorItem(SourceType sourceId, string topic, string label, string icon, string configKey, string prefix)
        {
            SourceId = sourceId;
            Topic = topic;
            Label = label;
            Icon = icon;
            ConfigKey = configKey;
            Prefix = prefix;
        }

        public abstract string Format(float? value);

        public override string ToString() => $"{SourceId}: {Label}";

        public static readonly IEnumerable<MonitorItem> MonitorItems = new List<MonitorItem>()
        {
            new NumericMonitorItem(SourceType.Framerate, "Framerate/FPS", "Framerate", Monitor, "Framerate", "FPS"),
            new NumericMonitorItem(SourceType.FramerateAverage, "Framerate/Average", "Average", Monitor, "Framerate", "FPS"),
            new NumericMonitorItem(SourceType.Framerate1Percent, "Framerate/1Percent", "1% Low", Monitor, "Framerate", "FPS"),

            new NumericMonitorItem(SourceType.CpuTemp, "CPU/Temperature", "CPU Temperature", Thermometer, "CPU:Temp", "CPU"),
            new NumericMonitorItem(SourceType.CpuUsage, "CPU/Usage", "CPU Usage", Processor, "CPU:Usage", "CPU"),
            new NumericMonitorItem(SourceType.CpuClock, "CPU/Clock", "CPU Clock Speed", Processor, "CPU:Clock", "CPU"),
            new NumericMonitorItem(SourceType.CpuPower, "CPU/Power", "CPU Power", LightningBolt, "CPU:Power", "CPU"),

            new NumericMonitorItem(SourceType.CommitCharge, "Memory/CommitCharge", "Commit Charge", Memory, "Memory:CommitCharge", "Memory"),
            new NumericMonitorItem(SourceType.MemoryUsage, "Memory/Used", "Memory Used", Memory, "Memory:Usage", "Memory"),

            new NumericMonitorItem(SourceType.GpuCoreClock, "GPU{0}/Clock", "Core Clock", ExpansionCard, "GPU:Clock", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuPower, "GPU{0}/Power", "Power", PowerPlug, "GPU:Power", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuShaderClock, "GPU{0}/ShaderClock", "Shader Clock", Drawing, "GPU:ShaderClock", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuCoreTemp, "GPU{0}/Temperature", "Core Temperature", Thermometer, "GPU:Temp", "GPU{0}"),
            new BooleanMonitorItem(SourceType.GpuTempLimit, "GPU{0}/TemperatureLimit", "Temperature Limit", Thermometer, "GPU:Temp", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuUsage, "GPU{0}/CoreUsage", "Core Usage", Speedometer, "GPU:Usage", "GPU{0}"),

            new NumericMonitorItem(SourceType.GpuCoreVoltage, "GPU{0}/PowerVoltage", "Core Voltage", LightningBolt, "GPU:Voltage", "GPU{0}", 3),
            new BooleanMonitorItem(SourceType.GpuCoreVoltageLimit, "GPU{0}/PowerVoltageLimit", "Voltage Limit", LightningBolt, "GPU:Voltage", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuPowerUsage, "GPU{0}/PowerUsage", "Power Usage", LightningBolt, "GPU:Power", "GPU{0}", 1),
            new BooleanMonitorItem(SourceType.GpuPowerLimit, "GPU{0}/PowerLimit", "Power Limit", LightningBolt, "GPU:Power", "GPU{0}"),
            new BooleanMonitorItem(SourceType.GpuLoadLimit, "GPU{0}/LoadLimit", "Load Limit", LightningBolt, "GPU:Power", "GPU{0}"),

            new NumericMonitorItem(SourceType.GpuFan1Speed, "GPU{0}/Fan1Speed", "Fan 1 Speed", Fan1, "GPU:FanSpeed", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuFan2Speed, "GPU{0}/Fan2Speed", "Fan 2 Speed", Fan2, "GPU:FanSpeed", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuFan3Speed, "GPU{0}/Fan3Speed", "Fan 3 Speed", Fan3, "GPU:FanSpeed", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuFan1Rpm, "GPU{0}/Fan1RPM", "Fan 1 RPM", Fan1, "GPU:FanSpeed", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuFan2Rpm, "GPU{0}/Fan2RPM", "Fan 2 RPM", Fan2, "GPU:FanSpeed", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuFan3Rpm, "GPU{0}/Fan3RPM", "Fan 3 RPM", Fan3, "GPU:FanSpeed", "GPU{0}"),

            new NumericMonitorItem(SourceType.GpuMemoryClock, "GPU{0}/MemoryClock", "Memory Clock", Memory, "GPU:MemoryClock", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuMemoryTemp, "GPU{0}/MemoryTemperature", "Memory Temperature", Thermometer, "GPU:MemoryTemp", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuMemoryUsed, "GPU{0}/MemoryUsed", "Memory Used", Memory, "GPU:MemoryUsed", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuMemoryVoltage, "GPU{0}/MemoryVoltage", "Memory Voltage", LightningBolt, "GPU:MemoryVoltage", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuFbUsage, "GPU{0}/FrameBuffer", "Frame Buffer Usage", Buffer, "GPU:FrameBufferUsage", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuVidUsage, "GPU{0}/VIDUsage", "Video Engine Load", Video, "GPU:VIDUsage", "GPU{0}"),
            new NumericMonitorItem(SourceType.GpuBusUsage, "GPU{0}/BusUsage", "Bus Usage", ExpansionCard, "GPU:BusUsage", "GPU{0}"),

            new NumericMonitorItem(SourceType.GpuVrmTemp, "GPU{0}/VRMTemperature", "VRM Temperature", Thermometer, "GPU:VRMTemp", "GPU{0}"),
        };
    }
}
