using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using QRCodeReaderAPI.Models;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QRCodeReaderAPI.Services
{
    public class QRReaderAppService: IQrReaderAppService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public QRReaderAppService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }     

        public async Task<string> ProcessQRImageAsync(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                var form = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(memoryStream.ToArray());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                form.Add(fileContent, "file", formFile.FileName);

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.PostAsync($"http://api.qrserver.com/v1/read-qr-code/", form);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                          
                var qrImageInterpretationResponse = JsonConvert.DeserializeObject<QRCodeInterpretationResponse[]>(responseContent);
                
                return qrImageInterpretationResponse.FirstOrDefault()?.Symbol.FirstOrDefault()?.Data;
            }
        }
    }
}
