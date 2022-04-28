using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BugTracker.Services.Interfaces
{
    public interface IBTFileService
    {
        public Task<byte[]> ConvertFileToBytnArrayAsync(IFormFile file);
        public string ConvertByteArrayToFile(byte[] fileData, string extenstion);
        public string GetFileIcon(string file);
        public string FormatFileSize(long bytes);
    }
}
