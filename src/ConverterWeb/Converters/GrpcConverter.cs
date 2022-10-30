using Grpc.Net.Client;
using Microsoft.Extensions.Options;

using ConverterService;
using ConverterWeb.Options;
using Google.Protobuf;
using Grpc.Core;

namespace ConverterWeb.Converters
{
    public class GrpcConverter : IConverter
    {
        private readonly GrpcHtmlConversionOptions _grpsOptions;

        public GrpcConverter(IOptions<GrpcHtmlConversionOptions> grpsOptions)
        {
            _grpsOptions = grpsOptions.Value;
        }

        public async Task<Guid> SendToConvertAsync(Stream stream)
        {
            var channel = GrpcChannel.ForAddress(_grpsOptions.Url);
            var client = new GrpcHtmlConversion.GrpcHtmlConversionClient(channel);

            var connection = client.ConvertHtml();
            var requestStream = connection.RequestStream;

            var bytes = new byte[4096];
            int size;

            stream.Position = 0;
            while ((size = stream.Read(bytes, 0, bytes.Length)) > 0)
            {
                await requestStream.WriteAsync(new HtmlConvertRequest
                {
                    Bytes = ByteString.CopyFrom(bytes, 0, size)
                });
            }

            await requestStream.CompleteAsync();

            var response = await connection.ResponseAsync;
            var guid = Guid.Parse(response.Guid);

            return guid;
        }
    }
}