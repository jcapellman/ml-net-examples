using Microsoft.ML.Data;

namespace chapter09.lib.ML.Objects
{
    public class FileData
    {
        [LoadColumn(0)]
        public float FileSize { get; set; }

        [LoadColumn(1)]
        public float Is64Bit { get; set; }

        [LoadColumn(2)]
        public float NumberImportFunctions { get; set; }

        [LoadColumn(3)]
        public float NumberExportFunctions { get; set; }

        [LoadColumn(4)]
        public float IsSigned { get; set; }

        [LoadColumn(5)]
        public float NumberImports { get; set; }

        [LoadColumn(6)]
        public bool Label { get; set; }

        public override string ToString() => $"{FileSize},{Is64Bit},{NumberImportFunctions},{NumberExportFunctions},{IsSigned},{NumberImports},{Label}";
    }
}