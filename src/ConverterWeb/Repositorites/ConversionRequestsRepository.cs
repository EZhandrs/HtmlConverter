using System.Collections.Concurrent;

namespace ConverterWeb.Repositorites
{
    public class ConversionRequestsRepository : IConversionRequestsRepository
    {
        ConcurrentDictionary<Guid, Bucket> _requests = new();

        public void Add(string userId, Guid fileId, string fileName)
        {
            _requests.TryAdd(fileId, new Bucket
            {
                UserId = userId,
                FileName = fileName
            });
        }

        public string GetFileName(Guid fileId)
        {
            _requests.TryGetValue(fileId, out Bucket bucket);
            return bucket.FileName;
        }

        public string GetUserId(Guid fileId)
        {
            _requests.TryGetValue(fileId, out Bucket bucket);
            return bucket.UserId;
        }

        private struct Bucket
        {
            public string UserId { get; set; }
            public string FileName { get; set; }
        }
    }
}