using Grpc.Core;

using ConverterService.FileProcessing;

namespace ConverterService.Grpc
{
    public class HtmlConversionService : GrpcHtmlConversion.GrpcHtmlConversionBase
    {
        private readonly ILogger<HtmlConversionService> _logger;
        private readonly IFileProcessor _fileProcessore;

        public HtmlConversionService(ILogger<HtmlConversionService> logger, IFileProcessor fileProcessor)
        {
            _logger = logger;
            _fileProcessore = fileProcessor;
        }

        public override async Task<HtmlConvertReply> ConvertHtml(IAsyncStreamReader<HtmlConvertRequest> requestStream, ServerCallContext context)
        {
            var stream = new MemoryStream();
            await foreach (var request in requestStream.ReadAllAsync())
            {
                stream.Write(request.Bytes.Memory.Span);
            }

            var guid = Guid.NewGuid();
            _logger.LogInformation("--> File received. Size: {Size}, FileId: {FileId}", stream.Length, guid);

            _fileProcessore.ProcessAsync(stream, guid);

            return new HtmlConvertReply
            {
                Guid = guid.ToString()
            };
        }
    }
}