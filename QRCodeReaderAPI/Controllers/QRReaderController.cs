using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCodeReaderAPI.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeReaderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QRReaderController: ControllerBase
    {
        private readonly IQrReaderAppService _qrReaderAppService;
        private readonly string[] SupportedFileTypes = new string[] { ".jpg", ".jpeg", ".png", ".gif" };

        public QRReaderController(IQrReaderAppService qrReaderAppService)
        {
            _qrReaderAppService = qrReaderAppService;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessQRCodeImage(IFormFile formFile)
        {
            if(!IsInputFileValid(formFile))
            {
                return BadRequest("Invalid file");
            }

            var qrCodeData = await _qrReaderAppService.ProcessQRImageAsync(formFile);

            return Ok(qrCodeData);
        }

        private bool IsInputFileValid(IFormFile formFile)
        {
            if (formFile.Length > 1048576 
                || !SupportedFileTypes.Contains(Path.GetExtension(formFile.FileName)))
            {
                return false;
            }

            return true;
        }

    }
}
