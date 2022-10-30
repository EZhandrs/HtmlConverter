using Microsoft.AspNetCore.Mvc;

using ConverterWeb.Repositorites;

namespace ConverterWeb.Controllers
{
    [Route("[controller]")]
    public class DownloadController : Controller
    {
        private readonly IPdfRepositority _pdfRepositority;
        private readonly IConversionRequestsRepository _requestsRepository;

        public DownloadController(IPdfRepositority pdfRepositority, IConversionRequestsRepository requestsRepository)
        {
            _pdfRepositority = pdfRepositority;
            _requestsRepository = requestsRepository;
        }

        [HttpGet]
        [Route("{fileId:guid}")]
        public IActionResult Index(Guid fileId)
        {
            var stream = _pdfRepositority.Get(fileId);
            var fileName = _requestsRepository.GetFileName(fileId);

            fileName = Path.GetFileNameWithoutExtension(fileName) + ".pdf";

            return File(stream, "application/pdf", fileName);
        }
    }
}