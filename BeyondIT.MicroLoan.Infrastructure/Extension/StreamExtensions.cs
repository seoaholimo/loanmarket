using System.IO;

namespace MoiFleet.Infrastructure.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToBytesArray(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}