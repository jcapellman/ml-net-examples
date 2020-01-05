using System.Linq;
using chapter09.Helpers;

namespace chapter09.Data
{
    public class FileClassificationResponseItem
    {
        private const float TRUE = 1.0f;
        private const float FALSE = 0.0f;

        public string SHA1Sum { get; set; }

        public double Confidence { get; set; }

        public bool IsMalicious { get; set; }

        public float IsLarge { get; set; }

        public float IsPE { get; set; }

        public float HasImports { get; set; }

        public FileClassificationResponseItem()
        {
        }

        public FileClassificationResponseItem(byte[] fileBytes)
        {
            SHA1Sum = fileBytes.ToSHA1();
            Confidence = 0.0;
            IsMalicious = true;

            IsPE = fileBytes.Take(2).ToString() == "MZ" ? TRUE : FALSE;
            IsLarge = fileBytes.Length > 65536 ? TRUE : FALSE;
            
        }
    }
}