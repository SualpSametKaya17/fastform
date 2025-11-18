using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class ConfigurationHistory
    {
        public int Id { get; set; }

        [Required]
        public int FormTemplateId { get; set; }

        [ForeignKey(nameof(FormTemplateId))]
        public FormTemplate FormTemplate { get; set; } = null!;

        [Required]
        public string ConfigurationJson { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ChangeDescription { get; set; }

        public int? ChangedById { get; set; }
        [ForeignKey(nameof(ChangedById))]
        public User? ChangedBy { get; set; }

        public DateTime ChangedDate { get; set; } = DateTime.Now;
    }
}
