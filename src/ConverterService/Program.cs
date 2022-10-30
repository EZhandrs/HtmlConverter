using ConverterService.FileProcessing;
using ConverterService.Grpc;
using ConverterWeb.Converters;
using ConverterWeb.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GrpcHtmlConvertedOptions>(
    builder.Configuration.GetSection(
        key: nameof(GrpcHtmlConvertedOptions)));

builder.Services.AddTransient<HtmlConverter>();
builder.Services.AddTransient<IFileProcessor, FileProcessor>();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<HtmlConversionService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
