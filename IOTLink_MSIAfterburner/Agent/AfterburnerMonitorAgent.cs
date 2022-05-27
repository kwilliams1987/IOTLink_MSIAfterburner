using IOTLinkAddon.Shared.Interop;
using IOTLinkAddon.Shared.MemoryManager;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Configs;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform;
using IOTLinkAPI.Platform.Events;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IOTLinkAddon.Agent
{
    public class AfterburnerMonitorAgent : AgentAddon
    {
        private Configuration _config;
        private SharedMemoryManager _sharedMemoryManager;

        public override void Init(IAddonManager addonManager)
        {
            Trace("Started");

            base.Init(addonManager);
            ReloadConfiguration();

            _sharedMemoryManager = new SharedMemoryManager();

            OnAgentRequestHandler += OnAgentRequest;
            OnConfigReloadHandler += OnConfigReload;

            Trace("Finished");
        }

        private void OnAgentRequest(object sender, AgentAddonRequestEventArgs e)
        {
            Trace("Started");

            if (_sharedMemoryManager is null)
            {
                Trace("Shared Memory Manager is not available, cancelling.");
                return;
            }

            var (gpus, metrics) = _sharedMemoryManager.GetAfterburnerData();
            
            foreach (var metric in metrics.Where(m => m.GpuIndex is null))
            {
                TraceLoop($"Sending metric data for {metric.ItemType.SourceId}.");
                GetManager().SendAgentResponse(this, new
                {
                    Type = ResponseType.Metric,
                    Data = new Metric
                    {
                        SourceId = metric.ItemType.SourceId,
                        Units = metric.Units,
                        Value = metric.Value,
                        Minimum = metric.Minimum,
                        Maximum = metric.Maximum,
                    }
                });
            }

            foreach (var gpu in gpus)
            {
                TraceLoop($"Sending metric data for GPU {gpu.Index}.");
                var gpuMetrics = metrics.Where(m => m.GpuIndex == gpu.Index);

                if (gpuMetrics.Any())
                {
                    GetManager().SendAgentResponse(this, new
                    {
                        Type = ResponseType.Gpu,
                        Data = new Gpu
                        {
                            Index = gpu.Index,
                            GpuId = gpu.GpuId,
                            Memory = gpu.Memory,
                            Driver = gpu.Driver,
                            Device = gpu.Device,
                            Family = gpu.Family,
                            BIOS = gpu.BIOS,

                            Metrics = gpuMetrics.Select(m => new Metric
                            {
                                SourceId = m.ItemType.SourceId,
                                Units = m.Units,
                                Value = m.Value,
                                Minimum = m.Minimum,
                                Maximum = m.Maximum,
                            }).ToList()
                        }
                    });
                }
            }

            Trace("Finished");
        }

        private void OnConfigReload(object sender, ConfigReloadEventArgs e)
        {
            if (e.ConfigType == ConfigType.CONFIGURATION_ADDON)
            {
                ReloadConfiguration();
            }
        }

        private void ReloadConfiguration()
        {
            var config = Path.Combine(_currentPath, "config.yaml");

            _config = ConfigurationManager.GetInstance().GetConfiguration(config);

            if (_config != null && _config.GetValue("enabled", false))
            {
                try
                {
                    _sharedMemoryManager = new SharedMemoryManager();
                }
                catch (InvalidOperationException ex)
                {
                    _sharedMemoryManager?.Dispose();
                    _sharedMemoryManager = null;

                    LoggerHelper.Error(ex.Message);
                }
                catch (FileNotFoundException)
                {
                    _sharedMemoryManager?.Dispose();
                    _sharedMemoryManager = null;

                    LoggerHelper.Info("MSI Afterburner is not available.");
                }
            }
            else
            {
                _sharedMemoryManager?.Dispose();
                _sharedMemoryManager = null;
            }
        }

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
