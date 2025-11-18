using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class BatchJob
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string JobName { get; set; } = string.Empty;

        [Required]
        public int FormTemplateId { get; set; }

        [ForeignKey(nameof(FormTemplateId))]
        public FormTemplate FormTemplate { get; set; } = null!;

        [MaxLength(500)]
        public string? DataSource { get; set; } // CSV, Excel file path

        public int? TotalRecords { get; set; }
        public int ProcessedRecords { get; set; } = 0;
        public int FailedRecords { get; set; } = 0;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Running, Completed, Failed, Cancelled

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? ErrorLog { get; set; }
    }
}
