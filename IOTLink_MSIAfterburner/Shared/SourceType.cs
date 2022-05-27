﻿namespace IOTLinkAddon.Shared
{
    public enum SourceType: uint
    {
        Unknown = uint.MaxValue,

        GpuCoreTemp = 0,
        GpuMemoryTemp = 2,
        GpuVrmTemp = 3,
        GpuFan1Speed = 16,
        GpuFan1Rpm = 17,
        GpuFan2Speed = 18,
        GpuFan2Rpm = 19,
        GpuFan3Speed = 20,
        GpuFan3Rpm = 21,
        GpuCoreClock = 32,
        GpuShaderClock = 33,
        GpuMemoryClock = 34,
        GpuUsage = 48,
        GpuMemoryUsed = 49,
        GpuFbUsage = 50,
        GpuVidUsage = 51,
        GpuBusUsage = 52,
        GpuCoreVoltage = 64,
        GpuMemoryVoltage = 66,
        Framerate = 80,
        Frametime = 81,
        FramerateMin = 82,
        FramerateAverage = 83,
        FramerateMax = 84,
        Framerate1Percent = 85,
        Framerate01Percent = 86,
        GpuPowerUsage = 96,
        GpuPower = 97,
        GpuTempLimit = 112,
        GpuPowerLimit = 113,
        GpuCoreVoltageLimit = 114,
        GpuLoadLimit = 116,
        CpuTemp = 128,
        CpuUsage = 144,
        MemoryUsage = 145,
        CommitCharge = 146,
        CpuClock = 160,
        CpuPower = 256,
    }
}
