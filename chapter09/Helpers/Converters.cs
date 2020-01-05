using chapter09.Data;
using chapter09.ML.Objects;

namespace chapter09.Helpers
{
    public static class Converters
    {
        public static FileData ToFileData(this FileClassificationResponseItem fileClassification)
        {
            return new FileData();
        }
    }
}