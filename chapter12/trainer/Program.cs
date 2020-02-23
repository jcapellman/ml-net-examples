using System;
using System.Linq;

using chapter12.lib.ML;

using chapter12.trainer.Enums;
using chapter12.trainer.Helpers;
using chapter12.trainer.Objects;

namespace chapter12.trainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Clear();

            var arguments = CommandLineParser.ParseArguments<ProgramArguments>(args);

            switch (arguments.Action)
            {
                case ProgramActions.PREDICT:
                    var predictor = new ImageClassificationPredictor();

                    var initialization = predictor.Initialize();

                    if (!initialization)
                    {
                        Console.WriteLine("Failed to initialize the model");

                        return;
                    }

                    var prediction = predictor.Predict(arguments.ImagePath);

                    Console.WriteLine($"Image ({arguments.ImagePath}) is classified as a {prediction.PredictedLabelValue} with a {prediction.Score.Max()}");
                    break;
                case ProgramActions.TRAINING:
                    new WebContentTrainer().Train(arguments.TrainingFileName, arguments.TestingFileName, arguments.ModelFileName);
                    break;
                default:
                    Console.WriteLine($"Unhandled action {arguments.Action}");
                    break;
            }
        }
    }
}