using System;
using System.Collections.Generic;
using System.Linq;

using chapter10_library.Enums;

namespace UWP_Browser_Classification_Trainer
{
    class Program
    {
        private static (TrainerActions Action, string pathOne, string pathTwo) ParseArgs(IReadOnlyList<string> args)
        {
            if (args.Count != 3)
            {
                throw new ArgumentException("Not enough arguments");
            }

            var action = Enum.GetNames(typeof(TrainerActions)).FirstOrDefault(a => a == args[0]);

            if (action == null)
            {
                throw new ArgumentOutOfRangeException($"{args[0]} is not a valid action");
            }

            return (Enum.Parse<TrainerActions>(action), args[1], args[2]);
        }

        static void Main(string[] args)
        {
            var (action, pathOne, pathTwo) = ParseArgs(args);

            switch (action)
            {
                case TrainerActions.TRAIN_MODEL:
                    Trainer.TrainModel(pathOne, pathTwo);
                    break;
                case TrainerActions.FEATURE_EXTRACTION:
                    FeatureExtract.Extract(pathOne, pathTwo);
                    break;
            }
        }
    }
}