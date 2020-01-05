using chapter09.lib.Data;
using chapter09.lib.ML.Objects;

namespace chapter09.lib.Helpers
{
    public static class Converters
    {
        public static FileData ToFileData(this FileClassificationResponseItem fileClassification)
        {
            return new FileData
            {
                IsLarge = fileClassification.IsLarge,
                HasImports = fileClassification.HasImports,
                IsPE = fileClassification.IsPE
            };
        }
    }
}