using System.IO;
using System.Threading.Tasks;

using chapter09.Data;
using chapter09.ML;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chapter09.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private FileClassificationPredictor _predictor;

        public UploadController(FileClassificationPredictor predictor)
        {
            _predictor = predictor;
        }

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

            var responseItem = new FileClassificationResponseItem(fileBytes);

            return _predictor.Predict(responseItem);
        }
    }
}