using Microsoft.AspNetCore.Mvc;

using ConverterWeb.Services;
using ConverterWeb.Dtos;

namespace ConverterWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHtmlConversionService _conversionService;

        public HomeController(IHtmlConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile formFile, string connectionId)
        {
            if (formFile == null)
            {
                return BadRequest("Choose file");
            }

            var extension = Path.GetExtension(formFile.FileName);

            if (extension != ".html")
            {
                return BadRequest("Invalid extension");
            }

            var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            var fileDto = new HtmlStreamDto
            {
                Stream = memoryStream,
                FileName = formFile.FileName
            };

            _conversionService.ConvertAsync(connectionId, fileDto);

            return Ok();
        }
    }
}