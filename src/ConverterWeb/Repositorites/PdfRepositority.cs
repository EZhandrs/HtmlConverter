using Microsoft.Extensions.Options;

using ConverterWeb.Options;

namespace ConverterWeb.Repositorites
{
    public class PdfRepositority : IPdfRepositority
    {
        private readonly PdfStorageOptions _pdfStorageOptions;

        public PdfRepositority(IOptions<PdfStorageOptions> pdfStorageOptions)
        {
            _pdfStorageOptions = pdfStorageOptions.Value;
        }

        public Stream Get(Guid fileId)
        {
            var path = Path.Combine(_pdfStorageOptions.Path, fileId.ToString());
            var stream = File.OpenRead(path);

            return stream;
        }

        public void Save(Stream stream, Guid fileId)
        {
            if (!Directory.Exists(_pdfStorageOptions.Path))
                Directory.CreateDirectory(_pdfStorageOptions.Path);

            var path = Path.Combine(_pdfStorageOptions.Path, fileId.ToString());

            using (var fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }
    }
}