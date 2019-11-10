using Microsoft.ML.Data;

namespace chapter06.ML.Objects
{
    public class LoginPrediction
    {
        [VectorType(3)]
        public double[] Prediction { get; set; }
    }
}