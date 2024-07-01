namespace DataExporter.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual Policy Policy { get; set; } = null!;
        public int PolicyId { get; set; }
    }
}
