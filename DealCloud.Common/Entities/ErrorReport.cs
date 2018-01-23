namespace DealCloud.Common.Entities
{
    public class ErrorReport
    {
        public int? ReferenceId { get; set; }

        public string Message { get; set; }

        public byte[] ReportData { get; set; }

        public override string ToString()
        {
            return $"{ReferenceId} {Message}";
        }
    }
}
