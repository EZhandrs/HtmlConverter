namespace ConverterWeb.Services
{
    public interface IHtmlConvertedService
    {
        Task ProcessResultAsync(Stream stream, Guid fileId);
    }
}