using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class FormTemplate
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImagePath { get; set; } = string.Empty;

        public byte[]? ImageData { get; set; }

        public byte[]? ThumbnailData { get; set; }

        [MaxLength(500)]
        public string? ConfigurationPath { get; set; }

        public int Version { get; set; } = 1;

        public bool IsActive { get; set; } = true;

        public bool IsPublic { get; set; } = false;

        // Navigation properties
        public int? CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public int? ModifiedById { get; set; }
        [ForeignKey(nameof(ModifiedById))]
        public User? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Tags { get; set; }

        // Collections
        public ICollection<FieldDefinition> FieldDefinitions { get; set; } = new List<FieldDefinition>();
    }
}
