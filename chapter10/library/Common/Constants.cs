using System;
using System.IO;

namespace chapter10.lib.Common
{
    public static class Constants
    {
        public static string MODEL_PATH = Path.Combine(AppContext.BaseDirectory, "fileclassification.mdl");

        public const string SAMPLE_DATA = "sampledata.csv";

        public const string TEST_DATA = "testdata.csv";
    }
}