using ConverterWeb.Dtos;
using ConverterWeb.Converters;
using ConverterWeb.Repositorites;

namespace ConverterWeb.Services
{
    public class HtmlConversionService : IHtmlConversionService
    {
        private readonly ILogger _logger;
        private readonly IConverter _convertService;
        private readonly IConversionRequestsRepository _requestsRepository;
        private readonly IClientNotificationService _notificationService;

        public HtmlConversionService(ILogger<HtmlConversionService> logger, IConverter convertService,
            IConversionRequestsRepository requestsRepository, IClientNotificationService notificationService)
        {
            _logger = logger;
            _convertService = convertService;
            _requestsRepository = requestsRepository;
            _notificationService = notificationService;
        }

        public async Task ConvertAsync(string userId, HtmlStreamDto htmlFileDto)
        {
            var guid = await _convertService.SendToConvertAsync(htmlFileDto.Stream);
            _logger.LogInformation("--> Sended to conversion. UserId: {UserId}, FileName: {FileName}, FileId: {FileId}", userId, htmlFileDto.FileName, guid);

            _requestsRepository.Add(userId, guid, htmlFileDto.FileName);

            await _notificationService.PdfConvertionAsync(guid);
        }
    }
}