using ConverterWeb.Dtos;

namespace ConverterWeb.Converters
{
    public interface IConverter
    {
        Task<Guid> SendToConvertAsync(Stream stream);
    }
}