using Microsoft.ML.Data;

namespace chapter10.lib.ML.Objects
{
    public class OutputItem
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}