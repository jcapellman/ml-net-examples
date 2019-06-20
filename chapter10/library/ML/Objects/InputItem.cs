using Microsoft.ML.Data;

namespace chapter10_library.ML.Objects
{
    public class InputItem
    {
        [LoadColumn(0)]
        public bool Label { get; set; }

        [LoadColumn(1)]
        public string HTMLContent { get; set; }
    }
}