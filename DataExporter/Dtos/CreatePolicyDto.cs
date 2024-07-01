using System.ComponentModel.DataAnnotations;

namespace DataExporter.Dtos
{
    public class CreatePolicyDto
    {
        [Required]
        public string PolicyNumber { get; set; } = null!;
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
        public IReadOnlyCollection<CreateNoteDto>? Notes { get; set; }
    }
}
