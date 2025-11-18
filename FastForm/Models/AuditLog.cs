using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Action { get; set; } = string.Empty; // Created, Updated, Deleted, Exported, Approved, etc.

        [Required]
        [MaxLength(100)]
        public string EntityType { get; set; } = string.Empty; // FormTemplate, FormInstance, FieldDefinition, etc.

        public int? EntityId { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
