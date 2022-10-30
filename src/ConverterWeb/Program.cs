using ConverterWeb.Services;
using ConverterWeb.Repositorites;
using ConverterWeb.Options;
using ConverterWeb.Converters;
using ConverterWeb.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<PdfStorageOptions>(
    builder.Configuration.GetSection(
        key: nameof(PdfStorageOptions)));

builder.Services.Configure<GrpcHtmlConversionOptions>(
builder.Configuration.GetSection(
    key: nameof(GrpcHtmlConversionOptions)));

builder.Services.AddSingleton<IConversionRequestsRepository, ConversionRequestsRepository>();
builder.Services.AddSingleton<IPdfRepositority, PdfRepositority>();
builder.Services.AddTransient<IClientNotificationService, ClientNotificationService>();
builder.Services.AddTransient<IConverter, GrpcConverter>();
builder.Services.AddTransient<IHtmlConversionService, HtmlConversionService>();
builder.Services.AddTransient<IHtmlConvertedService, HtmlConvertedService>();
builder.Services.AddSignalR();
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGrpcService<ConverterWeb.Grpc.HtmlConvertedService>();
app.MapGet("/grpc", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notification");
});

app.Run();
