namespace WebApplication1.Strategy
{
    public class FileConverter
    {
        private readonly IFileConversionStrategy _conversionStrategy;

        public FileConverter(IFileConversionStrategy conversionStrategy)
        {
            _conversionStrategy = conversionStrategy;
        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            return await _conversionStrategy.ConvertFileToByteArrayAsync(file);
        }
    }
}
