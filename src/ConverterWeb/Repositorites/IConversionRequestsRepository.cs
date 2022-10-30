namespace ConverterWeb.Repositorites
{
    public interface IConversionRequestsRepository
    {
        void Add(string userId, Guid fileId, string fileName);
        string GetFileName(Guid fileId);
        string GetUserId(Guid fileId);
    }
}