using System;

using chapter09.trainer.Enums;
using chapter09.trainer.Helpers;
using chapter09.trainer.Objects;

namespace chapter09.trainer
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
                    //new Predictor().Predict(arguments);
                    break;
                case ProgramActions.TRAINING:
                    //new Trainer().Train(arguments);
                    break;
                default:
                    Console.WriteLine($"Unhandled action {arguments.Action}");
                    break;
            }
        }
    }
}