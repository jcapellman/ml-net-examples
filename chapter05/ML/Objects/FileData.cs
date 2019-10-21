using Microsoft.ML.Data;

namespace chapter05.ML.Objects
{
    public class FileData
    {
        public FileData(byte[] data)
        {

        }

        [LoadColumn(0)] 
        public float Size { get; set; }

        [LoadColumn(1)]
        public float Header { get; set; }
    }
}