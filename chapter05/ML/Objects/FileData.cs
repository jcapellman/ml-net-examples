using System;
using System.Linq;

using chapter05.Enums;

namespace chapter05.ML.Objects
{
    public class FileData
    {
        public FileData(Span<byte> data, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Label = (float) FileTypes.Unknown;
            }
            else
            {
                if (fileName.Contains("ps1"))
                {
                    Label = (float) FileTypes.Script;
                } else if (fileName.Contains("exe"))
                {
                    Label = (float) FileTypes.Executable;
                } else if (fileName.Contains("doc"))
                {
                    Label = (float) FileTypes.Document;
                }
                else
                {
                    Label = (float) FileTypes.Unknown;
                }
            }

            Size = data.Length;

            Header = Avg(data.Slice(0, (int) (data.Length >= 200 ? 200 : Size)));

            IsText = HasBinaryContent(data) ? 1.0f : 0.0f;
        }

        private static float Avg(Span<byte> span) => (float) span.ToArray().Average(a => a);

        public static bool HasBinaryContent(Span<byte> fileContent) => 
            BitConverter.ToString(fileContent.ToArray()).Any(a => char.IsControl(a) && a != '\r' && a != '\n' && a != '\t');

        public float Label { get; set; }

        public float Size { get; set; }

        public float Header { get; set; }

        public float IsText { get; set; }

        public override string ToString() => $"{Label},{Size},{Header},{IsText}";
    }
}