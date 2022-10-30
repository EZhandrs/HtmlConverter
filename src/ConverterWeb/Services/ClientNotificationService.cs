using Microsoft.AspNetCore.SignalR;

using ConverterWeb.Dtos;
using ConverterWeb.Hubs;
using ConverterWeb.Repositorites;

namespace ConverterWeb.Services
{
    public class ClientNotificationService : IClientNotificationService
    {
        private readonly ILogger<ClientNotificationService> _logger;
        private readonly IConversionRequestsRepository _requestsRepository;
        private readonly IHubContext<NotificationHub> _hub;

        public ClientNotificationService(ILogger<ClientNotificationService> logger, IConversionRequestsRepository requestsRepository,
            IHubContext<NotificationHub> hub)
        {
            _logger = logger;
            _requestsRepository = requestsRepository;
            _hub = hub;
        }

        public async Task PdfConvertionAsync(Guid fileId)
        {
            var userId = _requestsRepository.GetUserId(fileId);
            var fileName = _requestsRepository.GetFileName(fileId);

            var dto = new PdfStateNotificationDto
            {
                FileId = fileId.ToString(),
                FileName = fileName
            };

            await _hub.Clients.Client(userId).SendAsync("PdfConvertion", dto);
        }

        public async Task PdfConvertedAsync(Guid fileId)
        {
            var userId = _requestsRepository.GetUserId(fileId);
            var fileName = _requestsRepository.GetFileName(fileId);

            fileName = Path.GetFileNameWithoutExtension(fileName) + ".pdf";

            var dto = new PdfStateNotificationDto
            {
                FileId = fileId.ToString(),
                FileName = fileName
            };

            await _hub.Clients.Client(userId).SendAsync("PdfConverted", dto);

            _logger.LogInformation("--> User notificated. UserId: {UserId}, FileId: {FileId}", userId, fileId);
        }
    }
}