using System;
using System.Linq;

using Microsoft.ML.Data;

namespace chapter05.ML.Objects
{
    public class FileData
    {
        public FileData(Span<byte> data)
        {
            Size = data.Length;

            Header = Avg(data.Slice(0, (int) (data.Length >= 200 ? 200 : Size)));
        }

        private static float Avg(Span<byte> span) => (float) span.ToArray().Average(a => a);

        [LoadColumn(0)] 
        public float Size { get; set; }

        [LoadColumn(1)]
        public float Header { get; set; }
    }
}