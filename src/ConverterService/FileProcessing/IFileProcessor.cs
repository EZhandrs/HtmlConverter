namespace ConverterService.FileProcessing
{
    public interface IFileProcessor
    {
        Task ProcessAsync(Stream stream, Guid fileId);
    }
}