using System;
using System.Linq;

using chapter05.Enums;

namespace chapter05.ML.Objects
{
    public class FileData
    {
        public FileData(Span<byte> data, string fileName = null)
        {
            if (!string.IsNullOrEmpty(fileName))
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
            }

            IsText = HasBinaryContent(data) ? 0.0f : 1.0f;

            IsMZHeader = HasHeaderBytes(data.Slice(0, 2), "MZ") ? 1.0f : 0.0f;

            IsPKHeader = HasHeaderBytes(data.Slice(0, 2), "PK") ? 1.0f : 0.0f;
        }

        public static bool HasBinaryContent(Span<byte> fileContent) =>
            System.Text.Encoding.UTF8.GetString(fileContent.ToArray()).Any(a => char.IsControl(a) && a != '\r' && a != '\n');

        public static bool HasHeaderBytes(Span<byte> data, string match) => System.Text.Encoding.UTF8.GetString(data) == match;

        public float Label { get; set; }

        public float IsText { get; set; }

        public float IsMZHeader { get; set; }

        public float IsPKHeader { get; set; }

        public override string ToString() => $"{Label},{IsText},{IsMZHeader},{IsPKHeader}";
    }
}