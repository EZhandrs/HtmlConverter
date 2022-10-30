using ConverterWeb.Repositorites;

namespace ConverterWeb.Services
{
    public class HtmlConvertedService : IHtmlConvertedService
    {
        private readonly IPdfRepositority _pdfRepositority;
        private readonly IClientNotificationService _notificationService;

        public HtmlConvertedService(IPdfRepositority pdfRepositority, IClientNotificationService notificationService)
        {
            _pdfRepositority = pdfRepositority;
            _notificationService = notificationService;
        }

        public async Task ProcessResultAsync(Stream stream, Guid fileId)
        {
            _pdfRepositority.Save(stream, fileId);
            await _notificationService.PdfConvertedAsync(fileId);
        }
    }
}