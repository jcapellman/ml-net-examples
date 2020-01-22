using chapter10.lib.Data;
using chapter10.lib.ML.Objects;

namespace chapter10.lib.Helpers
{
    public static class Converters
    {
        public static FileData ToFileData(this FileClassificationResponseItem fileClassification)
        {
            return new FileData
            {
                Is64Bit = fileClassification.Is64Bit,
                IsSigned = fileClassification.IsSigned,
                NumberImports = fileClassification.NumImports,
                NumberImportFunctions = fileClassification.NumImportFunctions,
                NumberExportFunctions = fileClassification.NumExportFunctions,
                FileSize = fileClassification.FileSize,
                Strings = fileClassification.Strings
            };
        }
    }
}