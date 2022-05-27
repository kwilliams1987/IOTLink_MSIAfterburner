using IOTLinkAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IOTLinkAddon.Shared.MemoryManager
{
    public class SharedMemoryManager: IDisposable
    {
        private static readonly string SHARED_MEMORY_ID = "MAHMSharedMemory";

        private const int stringLength = 260;

        private MemoryMappedFile _sharedMemory;
        private MemoryMappedViewAccessor _viewAccessor;
        private long offset = 0;

        public SharedMemoryManager()
        {
            Trace("Started");

            try
            {
                _sharedMemory = MemoryMappedFile.OpenExisting(SHARED_MEMORY_ID, MemoryMappedFileRights.Read);
                _viewAccessor = _sharedMemory.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
            }
            catch(FileNotFoundException)
            {
                _sharedMemory?.Dispose();
                _viewAccessor?.Dispose();

                _sharedMemory = null;
                _viewAccessor = null;

                throw;
            }
            catch(Exception ex)
            {
                _sharedMemory?.Dispose();
                _viewAccessor?.Dispose();

                _sharedMemory = null;
                _viewAccessor = null;

                throw new InvalidOperationException("Unexpected error connecting to MSI Afterburner.", ex);
            }

            Trace("Finished");
        }

        public (IEnumerable<AfterburnerGpu> Gpus, IEnumerable<AfterburnerMetric> Metrics) GetAfterburnerData()
        {
            Trace("Started");
            if (_sharedMemory is null)
            {
                Trace("Shared Memory is not available.");
                return (Enumerable.Empty<AfterburnerGpu>(), Enumerable.Empty<AfterburnerMetric>());
            }

            offset = 0;

            var header = new AfterburnerHeader();
            _viewAccessor?.Read(offset, out header);

            if (header.HeaderSize == 0 || header.EntryCount == 0)
            {
                return (Enumerable.Empty<AfterburnerGpu>(), Enumerable.Empty<AfterburnerMetric>());
            }

            offset += header.HeaderSize;

            var metrics = GetMetrics(header.EntryCount).ToList();
            var gpus = GetGpus(header.GpuEntryCount).ToList();


            Trace("Finished");
            return (gpus, metrics);
        }

        private IEnumerable<AfterburnerMetric> GetMetrics(uint entries)
        {
            for (var e = 0; e < entries; e++)
            {
                TraceLoop($"Parsing metric entry {e}.");

                string name = string.Empty,
                   //localName = string.Empty, 
                   //localUnits = string.Empty, 
                   //format = string.Empty,
                   units = string.Empty;

                float? value = 0;
                float min = 0,
                    max = 0;

                uint? gpuIndex = 0;
                uint //flags = 0,
                    sourceId = 0;

                _viewAccessor?.ReadString(offset, out name, stringLength);
                offset += stringLength;

                _viewAccessor?.ReadString(offset, out units, stringLength);
                offset += stringLength;

                // _viewAccessor?.ReadString(offset, out localName, charArrayWidth);
                offset += stringLength;

                // _viewAccessor?.ReadString(offset, out localUnits, charArrayWidth);
                offset += stringLength;

                // _viewAccessor?.ReadString(offset, out format, charArrayWidth);
                offset += stringLength;

                _viewAccessor?.Read(offset, out value);
                offset += sizeof(float);

                _viewAccessor?.Read(offset, out min);
                offset += sizeof(float);

                _viewAccessor?.Read(offset, out max);
                offset += sizeof(float);

                // _viewAccessor?.Read(offset, out uint flags);
                offset += sizeof(uint);

                _viewAccessor?.Read(offset, out gpuIndex);
                offset += sizeof(uint);

                _viewAccessor?.Read(offset, out sourceId);
                offset += sizeof(uint);

                var sourceType = (SourceType)sourceId;

                if (sourceType == SourceType.Unknown)
                {
                    Console.WriteLine("Unknown MSI Afterburner metric SourceId {0}: {1}, {2}, {3}", sourceId, name, value, units);
                    continue;
                }

                var itemType = MonitorItem.MonitorItems.Find(sourceType);

                if (itemType == default)
                {
                    Console.WriteLine("{0} {1} isn't mapped to a {2} (name: {3}, units: {4}, current: {5})",
                        nameof(SourceType), sourceType, nameof(MonitorItem), name, units, value);

                    continue;
                }

                if (gpuIndex == uint.MaxValue)
                {
                    gpuIndex = null;
                }

                // Custom metric hacks.
                switch (sourceType)
                {
                    // Commit Charge and MemoryUsage should not be attached to a GPU.
                    case SourceType.CommitCharge:
                    case SourceType.MemoryUsage:
                        gpuIndex = null;
                        break;
                    case SourceType.Framerate:
                    case SourceType.FramerateMin:
                    case SourceType.FramerateAverage:
                    case SourceType.FramerateMax:
                    case SourceType.Framerate1Percent:
                    case SourceType.Framerate01Percent:
                        // When Framerate is not active it returns float.MaxValue
                        value = (value == float.MaxValue ? null : value);
                        break;
                }

                yield return new AfterburnerMetric()
                {
                    GpuIndex = gpuIndex,
                    ItemType = itemType,
                    Units = units,
                    Value = value,
                    Minimum = min,
                    Maximum = max
                };
            }
        }

        private IEnumerable<AfterburnerGpu> GetGpus(uint entries)
        {
            for (var g = 0; g < entries; g++)
            {
                TraceLoop($"Parsing GPU entry {g}.");

                string id = string.Empty,
                    family = string.Empty,
                    device = string.Empty,
                    driver = string.Empty,
                    bios = string.Empty;

                uint memAmount = 0;

                _viewAccessor?.ReadString(offset, out id, stringLength);
                offset += stringLength;

                _viewAccessor?.ReadString(offset, out family, stringLength);
                offset += stringLength;

                _viewAccessor?.ReadString(offset, out device, stringLength);
                offset += stringLength;

                _viewAccessor?.ReadString(offset, out driver, stringLength);
                offset += stringLength;

                _viewAccessor?.ReadString(offset, out bios, stringLength);
                offset += stringLength;

                _viewAccessor?.Read(offset, out memAmount);
                offset += sizeof(uint);

                yield return new AfterburnerGpu
                {
                    BIOS = bios,
                    Device = device,
                    Driver = driver,
                    Family = family,
                    GpuId = id,
                    Index = g,
                    Memory = memAmount
                };
            }
        }

        ~SharedMemoryManager()
        {
            Dispose();
        }

        public void Dispose()
        {
            _viewAccessor?.Dispose();
            _sharedMemory?.Dispose();

            _viewAccessor = null;
            _sharedMemory = null;

            GC.SuppressFinalize(this);
        }

#pragma warning disable CS0649
        internal struct AfterburnerHeader
        {
            public uint Signature;
            public uint Version;
            public uint HeaderSize;
            public uint EntryCount;
            public uint EntrySize;
            public uint Time;
            public uint GpuEntryCount;
            public uint GpuEntrySize;
        }
#pragma warning restore CS0649

        private void Trace(string message, [CallerMemberName] string caller = "")
        {
            LoggerHelper.Trace("{0}::{1}() - {2}", GetType().Name, caller, message);
        }

        private void TraceLoop(string message, [CallerMemberName] string caller = "")
        {
            LoggerHelper.TraceLoop("{0}::{1}() - {2}", GetType().Name, caller, message);
        }
    }
}
