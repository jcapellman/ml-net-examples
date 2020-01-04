namespace chapter09.Data
{
    public class FileClassificationResponseItem
    {
        public string SHA1Sum { get; set; }

        public double Confidence { get; set; }

        public bool IsMalicious { get; set; }
    }
}