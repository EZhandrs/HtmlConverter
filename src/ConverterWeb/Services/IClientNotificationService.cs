namespace ConverterWeb.Services
{
    public interface IClientNotificationService
    {
        Task PdfConvertionAsync(Guid fileId);
        Task PdfConvertedAsync(Guid fileId);
    }
}