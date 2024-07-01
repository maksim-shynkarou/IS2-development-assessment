using System.ComponentModel.DataAnnotations;

namespace DataExporter.Dtos
{
    public class CreateNoteDto
    {
        [Required]
        public string Text { get; set; } = null!;
    }
}
