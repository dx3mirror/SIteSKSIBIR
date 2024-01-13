namespace WebApplication1.Strategy
{
    public class MemoryConversionStrategy : IFileConversionStrategy
    {
        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
