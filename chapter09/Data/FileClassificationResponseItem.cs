using chapter09.Helpers;

namespace chapter09.Data
{
    public class FileClassificationResponseItem
    {
        public string SHA1Sum { get; set; }

        public double Confidence { get; set; }

        public bool IsMalicious { get; set; }

        public FileClassificationResponseItem()
        {
        }

        public FileClassificationResponseItem(byte[] fileBytes)
        {
            SHA1Sum = fileBytes.ToSHA1();
            Confidence = 0.0;
            IsMalicious = true;
        }
    }
}