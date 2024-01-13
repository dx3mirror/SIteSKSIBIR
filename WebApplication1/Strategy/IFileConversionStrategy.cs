namespace WebApplication1.Strategy
{
    public interface IFileConversionStrategy
    {
        Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);
    }
}
