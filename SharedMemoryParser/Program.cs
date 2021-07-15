using IOTLinkAddon.Shared.MemoryManager;
using System;
using System.Linq;

namespace SharedMemoryParser
{
    class Program
    {
        static void Main()
        {
            using (var memoryManager = new SharedMemoryManager())
            {
                var (gpus, metrics) = memoryManager.GetAfterburnerData();

                foreach (var metric in metrics.Where(m => m.GpuIndex == uint.MaxValue))
                {
                    Console.WriteLine("Found metric: {0}, {1}{2}", metric.ItemType.SourceId, metric.Value, metric.Units);
                }

                Console.WriteLine();
                foreach (var gpu in gpus)
                {
                    Console.WriteLine("Found GPU: {0}, {1}, {2}", gpu.Index, gpu.GpuId, gpu.Device);

                    if (!string.IsNullOrWhiteSpace(gpu.Family))
                    {
                        Console.WriteLine("Family: {0}", gpu.Family);
                    }

                    if (!string.IsNullOrWhiteSpace(gpu.Driver))
                    {
                        Console.WriteLine("Driver: {0}", gpu.Driver);
                    }

                    if(gpu.Memory > 0)
                    {
                        Console.WriteLine("Memory: {0}", gpu.Memory);
                    }

                    foreach (var metric in metrics.Where(m => m.GpuIndex == gpu.Index))
                    {
                        Console.WriteLine("Found metric: {0}, {1}{2}", metric.ItemType.SourceId, metric.Value, metric.Units);
                    }


                    Console.WriteLine();
                }
            }
        }
    }
}
