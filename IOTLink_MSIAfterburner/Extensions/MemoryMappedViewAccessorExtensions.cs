using System.Text;

namespace System.IO.MemoryMappedFiles
{
    static class MemoryMappedViewAccessorExtensions
    {
        public static void ReadString(this MemoryMappedViewAccessor accessor, long position, out string result, int maxLength)
        {
            var bytes = new byte[maxLength];
            accessor.ReadArray(position, bytes, 0, maxLength);
            result = Encoding.UTF7.GetString(bytes).TrimEnd('\0');
        }

        public static void Read<T>(this MemoryMappedViewAccessor accessor, long position, out T? result) where T : struct
        {
            accessor.Read(position, out T value);
            result = value;
        }
    }
}
