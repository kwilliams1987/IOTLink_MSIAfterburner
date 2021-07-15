using IOTLinkAddon.Shared;
using IOTLinkAddon.Shared.Interop;
using IOTLinkAPI.Addons;
using IOTLinkAPI.Configs;
using IOTLinkAPI.Helpers;
using IOTLinkAPI.Platform;
using IOTLinkAPI.Platform.Events;
using IOTLinkAPI.Platform.Events.MQTT;
using IOTLinkAPI.Platform.HomeAssistant;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using static IOTLinkAddon.Shared.Icons;

namespace IOTLinkAddon.Service
{
    public class AfterburnerMonitorService : ServiceAddon
    {
        private const int DefaultInterval = 10;
        private Configuration _config;
        private readonly Timer _timer = new Timer();

        public override void Init(IAddonManager addonManager)
        {
            Trace("Started");
            
            base.Init(addonManager);
            ReloadConfiguration();

            OnConfigReloadHandler += OnConfigReload;
            OnAgentResponseHandler += OnAgentResponse;
            OnMQTTConnectedHandler += OnMQTTConnected;
            OnMQTTDisconnectedHandler += OnMQTTDisconnected;

            StartTimer();

            Trace("Finished");
        }

        private void OnMQTTConnected(object sender, MQTTEventEventArgs e)
        {
            Trace("Started");

            StartTimer();
            RequestAgentMetrics();

            Trace("Finished");
        }

        private void OnMQTTDisconnected(object sender, MQTTEventEventArgs e)
        {
            Trace("Started");

            _timer.Stop();

            Trace("Finished");
        }

        private void StartTimer()
        {
            Trace("Started");
            
            _timer.Stop();

            _timer.Elapsed -= OnTimerElapsed;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = _config.GetValue("interval", 5000);

            _timer.Start();

            Trace("Finished");
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Trace("Started");

            _timer.Stop();

            try
            {
                RequestAgentMetrics();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Failed to request Agent Metrics: {0}.", ex);
            }
            finally
            {
                _timer.Start();
            }

            Trace("Finished");
        }

        private void RequestAgentMetrics()
        {
            Trace("Started");

            GetManager().SendAgentRequest(this, new object());

            Trace("Finished");
        }

        private void OnConfigReload(object sender, ConfigReloadEventArgs e)
        {
            Trace("Started");

            if (e.ConfigType == ConfigType.CONFIGURATION_ADDON)
            {
                ReloadConfiguration();
            }

            Trace("Finished");
        }

        private void OnAgentResponse(object sender, AgentAddonResponseEventArgs e)
        {
            Trace("Started");

            var responseType = (ResponseType)e.Data.Type;

            switch (responseType)
            {
                case ResponseType.Gpu:
                    PublishGpuData(e.Data.Data);
                    break;

                case ResponseType.Metric:
                    PublishMetricData(e.Data.Data);
                    break;

                default:
                    LoggerHelper.Verbose("Unsupported ResponseType {0} was returned.", responseType);
                    break;
            }

            Trace("Finished");
        }

        private void PublishGpuData(dynamic response)
        {
            Gpu gpu;

            var disabledGpus = _config.GetList<int>("ignore_devices");

            try
            {
                gpu = (Gpu)response.ToObject<Gpu>();
            }
            catch (Exception ex) 
            { 
                LoggerHelper.Warn("Unable to parse returned response into {0} struct: {1}.", nameof(Gpu), ex.Message);
                return;
            }

            if (gpu.Metrics.Any(m => m.IsEnabled(_config)) && !disabledGpus.Contains(gpu.Index))
            {
                var prefix = $"GPU{gpu.Index}";
                const string labelFormat = "{0}: {1}";

                PublishDiscoveryAndValue($"{prefix}/Name", ExpansionCard, null, string.Format(labelFormat, prefix, "Name"), gpu.Device, prefix);

                if (!string.IsNullOrWhiteSpace(gpu.Driver))
                {
                    PublishDiscoveryAndValue($"{prefix}/Driver", DriverFile, null, string.Format(labelFormat, prefix, "Driver Version"), gpu.Driver, prefix);
                }

                if (!string.IsNullOrWhiteSpace(gpu.Family))
                {
                    PublishDiscoveryAndValue($"{prefix}/Family", ExpansionCard, null, string.Format(labelFormat, prefix, "Device Family"), gpu.Family, prefix);
                }

                if (gpu.Memory != 0)
                {
                    var used = gpu.Metrics.FirstOrDefault(m => m.SourceId == SourceType.GpuMemoryUsed);

                    if (used.Value.HasValue && used.Value != 0 && used.IsEnabled(_config))
                    {
                        PublishDiscoveryAndValue($"{prefix}/MemoryTotal", Memory, "MB", string.Format(labelFormat, prefix, "Total Memory"), gpu.Memory, prefix, 0);
                        PublishDiscoveryAndValue($"{prefix}/MemoryUsage", Memory, "%", string.Format(labelFormat, prefix, "Memory Usage"), used.Value.Value / gpu.Memory, prefix, 0);
                        PublishDiscoveryAndValue($"{prefix}/MemoryAvailable", Memory, "MB", string.Format(labelFormat, prefix, "Available Memory"), gpu.Memory - used.Value.Value, prefix, 0);
                    }
                }

                foreach (var metric in gpu.Metrics.WhereEnabled(_config))
                {
                    var metadata = MonitorItem.MonitorItems.FirstOrDefault(m => m.SourceId == metric.SourceId);

                    switch (metric.SourceId)
                    {
                        case SourceType.GpuTempLimit:
                        case SourceType.GpuPowerLimit:
                            PublishDiscoveryAndValue(string.Format(metadata.Topic, gpu.Index), metadata.Icon, null, string.Format(labelFormat, prefix, metadata.Label), metric.Value == 0 ? "off" : "on", string.Format(metadata.Prefix, gpu.Index));
                            break;
                        default:
                            PublishDiscoveryAndValue(string.Format(metadata.Topic, gpu.Index), metadata.Icon, metric.Units, string.Format(labelFormat, prefix, metadata.Label), metric.Value, string.Format(metadata.Prefix, gpu.Index), metadata.Precision);
                            break;
                    }                    
                }
            }
        }

        private void PublishMetricData(dynamic response)
        {
            Metric metric;

            try
            {
                metric = (Metric)response.ToObject<Metric>();
            }
            catch (Exception ex)
            {
                LoggerHelper.Warn("Unable to parse returned response into {0} struct: {1}.", nameof(Metric), ex.Message);
                return;
            }

            if (metric.IsEnabled(_config))
            {
                var metadata = MonitorItem.MonitorItems.FirstOrDefault(m => m.SourceId == metric.SourceId);
                
                PublishDiscoveryAndValue(metadata.Topic, metadata.Icon, metric.Units, metadata.Label, metric.Value, metadata.Prefix, metadata.Precision);
            }
        }

        private void PublishDiscoveryAndValue(string topic, string icon, string units, string label, float? value, string prefix, int precision)
            => PublishDiscoveryAndValue(topic, icon, units, label, value?.ToString($"F{precision}"), prefix);

        private void PublishDiscoveryAndValue(string topic, string icon, string units, string label, string value, string prefix)
        {
            topic = $"Stats/{topic}";
            PublishDiscovery(topic, icon, units, label, value, prefix);
            PublishItem(topic, value);
        }

        private void PublishDiscovery(string topic, string icon, string units, string label, string value, string prefix)
        {
            GetManager().PublishDiscoveryMessage(this, topic, prefix, new HassDiscoveryOptions()
            {
                Id = topic.Split('/').Last(),
                Icon = icon,
                Unit = units,
                Name = label,
                Component = HomeAssistantComponent.Sensor
            });
        }

        private void PublishItem(string topic, string value)
        {
            GetManager().PublishMessage(this, topic, value);
        }

        private void ReloadConfiguration()
        {
            Trace("Started");

            var config = Path.Combine(_currentPath, "config.yaml");
            _config = ConfigurationManager.GetInstance().GetConfiguration(config);

            Trace("Finished");
        }

        private void Trace(string message, [CallerMemberName] string caller = "")
        {
            LoggerHelper.Trace("{0}::{1}() - {2}", GetType().Name, caller, message);
        }
    }
}
