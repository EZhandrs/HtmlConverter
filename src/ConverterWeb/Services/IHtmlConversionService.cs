using ConverterWeb.Dtos;

namespace ConverterWeb.Services
{
    public interface IHtmlConversionService
    {
        Task ConvertAsync(string userId, HtmlStreamDto htmlDto);
    }
}