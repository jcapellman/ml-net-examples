using Microsoft.ML.Data;

namespace chapter03.ML.Objects
{
    public class EmploymentHistoryPrediction
    {
        [ColumnName("Score")]
        public float DurationInMonths;

        [ColumnName("Probability")]
        public float Probability { get; set; }
    }
}