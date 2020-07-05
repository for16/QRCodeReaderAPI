using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace QRCodeReaderAPI.Services
{
    public interface IQrReaderAppService
    {
        Task<string> ProcessQRImageAsync(IFormFile formFile);
    }
}
