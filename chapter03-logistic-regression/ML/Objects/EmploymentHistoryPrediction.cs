using Microsoft.ML.Data;

namespace chapter03_logistic_regression.ML.Objects
{
    public class EmploymentHistoryPrediction
    {
        [ColumnName("Score")]
        public float DurationInMonths;
    }
}