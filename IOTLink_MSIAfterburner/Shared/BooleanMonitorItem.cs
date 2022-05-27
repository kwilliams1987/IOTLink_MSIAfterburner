using System;

namespace IOTLinkAddon.Shared
{
    public class BooleanMonitorItem : MonitorItem
    {
        public bool Invert { get; private set; }

        public BooleanMonitorItem(SourceType sourceId, string topic, string label, string icon, string configKey, string prefix, bool invert = false)
            : base(sourceId, topic, label, icon, configKey, prefix)
        {
            Invert = invert;
        }

        public override string Format(float? value)
        {
            if (value is null)
            {
                return null;
            }

            if (Invert)
            {
                if (value == 0)
                {
                    return "On";
                }

                return "Off";
            }
            else if (value == 0)
            {
                return "Off";
            }

            return "On";            
        }
    }
}
