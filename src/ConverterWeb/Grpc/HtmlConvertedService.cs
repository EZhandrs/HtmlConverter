using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

using ConverterService;
using ConverterWeb.Services;

namespace ConverterWeb.Grpc
{
    public class HtmlConvertedService : GrpcHtmlConverted.GrpcHtmlConvertedBase
    {
        private readonly ILogger<HtmlConvertedService> _logger;
        private readonly IHtmlConvertedService _htmlConvertedService;

        public HtmlConvertedService(ILogger<HtmlConvertedService> logger, IHtmlConvertedService htmlConvertedService)
        {
            _logger = logger;
            _htmlConvertedService = htmlConvertedService;
        }

        public override async Task<Empty> ConvertedHtml(HtmlConvertedRequest request, ServerCallContext context)
        {
            _logger.LogInformation("--> File converted. FileId: {FileId}, Size: {Size}", request.Guid, request.Bytes.Length);

            var pdfStream = new MemoryStream();
            pdfStream.Write(request.Bytes.Span);

            _htmlConvertedService.ProcessResultAsync(pdfStream, Guid.Parse(request.Guid));

            return new Empty();
        }
    }
}