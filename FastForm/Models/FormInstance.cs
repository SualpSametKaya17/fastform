using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class FormInstance
    {
        public int Id { get; set; }

        [Required]
        public int FormTemplateId { get; set; }

        [ForeignKey(nameof(FormTemplateId))]
        public FormTemplate FormTemplate { get; set; } = null!;

        [MaxLength(200)]
        public string? InstanceName { get; set; }

        [MaxLength(50)]
        public string? InstanceNumber { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Draft"; // Draft, InProgress, Completed, Approved, Rejected, Archived

        [MaxLength(20)]
        public string Priority { get; set; } = "Normal"; // Low, Normal, High, Urgent

        // Ownership
        public int? CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public int? AssignedToId { get; set; }
        [ForeignKey(nameof(AssignedToId))]
        public User? AssignedTo { get; set; }

        public int? ApprovedById { get; set; }
        [ForeignKey(nameof(ApprovedById))]
        public User? ApprovedBy { get; set; }

        // Dates
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public DateTime? CompletedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? DueDate { get; set; }

        // Output
        [MaxLength(500)]
        public string? OutputPath { get; set; }

        [MaxLength(50)]
        public string? OutputFormat { get; set; } // PDF, PNG, DOCX

        // Metadata
        public string? Notes { get; set; }

        [MaxLength(500)]
        public string? Tags { get; set; }

        // Collections
        public ICollection<FieldValue> FieldValues { get; set; } = new List<FieldValue>();
    }
}
