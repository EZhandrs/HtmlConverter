using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

using ConverterWeb.Options;
using ConverterWeb.Converters;

namespace ConverterService.FileProcessing
{
    public class FileProcessor : IFileProcessor
    {
        private readonly ILogger<FileProcessor> _logger;
        private readonly HtmlConverter _converter;
        private readonly GrpcHtmlConvertedOptions _grpsOptions;

        public FileProcessor(ILogger<FileProcessor> logger, HtmlConverter converter, IOptions<GrpcHtmlConvertedOptions> grpsOptions)
        {
            _logger = logger;
            _converter = converter;
            _grpsOptions = grpsOptions.Value;
        }

        public async Task ProcessAsync(Stream stream, Guid fileId)
        {
            var pdfStream = await _converter.ConvertAsync(stream);
            _logger.LogInformation("--> File converted. FileId: {FileId}, Size: {Size}", fileId, pdfStream.Length);

            var channel = GrpcChannel.ForAddress(_grpsOptions.Url);
            var client = new GrpcHtmlConverted.GrpcHtmlConvertedClient(channel);

            var request = new HtmlConvertedRequest
            {
                Guid = fileId.ToString(),
                Bytes = ByteString.FromStream(pdfStream)
            };

            client.ConvertedHtml(request);

            _logger.LogInformation("--> File sended. FileId: {FileId}, Size: {Size}", fileId, request.Bytes.Length);
        }
    }
}