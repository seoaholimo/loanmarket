namespace BeyondIT.MicroLoan.Infrastructure.Compression
{
    public interface ICompressionService
    {
        byte[] CompressFile(byte[] fileBytes);
        byte[] DeCompressFile(byte[] fileBytes);
    }
}