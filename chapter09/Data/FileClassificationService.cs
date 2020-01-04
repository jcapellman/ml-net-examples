using System.Threading.Tasks;

namespace chapter09.Data
{
    public class FileClassificationService
    {
        public async Task<FileClassificationResponseItem> GetFileClassificationAsync(string sha1Sum)
        {
            return new FileClassificationResponseItem();
        }
    }
}