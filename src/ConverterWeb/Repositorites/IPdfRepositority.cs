using ConverterWeb.Dtos;

namespace ConverterWeb.Repositorites
{
    public interface IPdfRepositority
    {
        Stream Get(Guid fileId);
        void Save(Stream stream, Guid fileId);
    }
}