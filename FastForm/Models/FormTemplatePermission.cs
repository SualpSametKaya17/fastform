using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class FormTemplatePermission
    {
        public int Id { get; set; }

        [Required]
        public int FormTemplateId { get; set; }

        [ForeignKey(nameof(FormTemplateId))]
        public FormTemplate FormTemplate { get; set; } = null!;

        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [MaxLength(50)]
        public string? RoleName { get; set; }

        public bool CanView { get; set; } = true;
        public bool CanEdit { get; set; } = false;
        public bool CanDelete { get; set; } = false;
        public bool CanApprove { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
