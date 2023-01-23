using System.IO;
using System.IO.Compression;

namespace BeyondIT.MicroLoan.Infrastructure.Compression
{
    public class CompressionService : ICompressionService
    {
        public byte[] CompressFile(byte[] fileBytes)
        {
            using (var originalFileStream = new MemoryStream(fileBytes))
            {
                using (var compressedFileStream = new MemoryStream())
                {
                    using (var compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }

                    return compressedFileStream.ToArray();
                }
            }
        }

        public byte[] DeCompressFile(byte[] fileBytes)
        {
            using (var compressedFileStream = new MemoryStream(fileBytes))
            {
                using (var decompressedFileStream = new MemoryStream())
                {
                    using (var decompressionStream = new DeflateStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }

                    return decompressedFileStream.ToArray();
                }
            }
        }
    }
}