using Microsoft.ML.Data;

namespace chapter09.lib.ML.Objects
{
    public class FileData
    {
        [LoadColumn(0)]
        public float IsLarge { get; set; }

        [LoadColumn(1)]
        public float IsPE { get; set; }

        [LoadColumn(2)]
        public float HasImports { get; set; }

        [LoadColumn(3)]
        public bool Label { get; set; }

        public override string ToString() => $"{IsLarge},{IsPE},{HasImports},{Label}";
    }
}