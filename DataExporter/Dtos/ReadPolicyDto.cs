namespace DataExporter.Dtos
{
    public class ReadPolicyDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
        public IReadOnlyCollection<NoteDto> Notes { get; set; }
    }
}
