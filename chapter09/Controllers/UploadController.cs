using System.IO;
using System.Threading.Tasks;

using chapter09.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chapter09.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private static byte[] GetBytesFromPost(IFormFile file)
        {
            using (var ms = new BinaryReader(file.OpenReadStream()))
            {
                return ms.ReadBytes((int)file.Length);
            }
        }

        [HttpPost]
        public async Task<FileClassificationResponseItem> Post(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }

            var fileBytes = GetBytesFromPost(file);

            return new FileClassificationResponseItem(fileBytes);
        }
    }
}