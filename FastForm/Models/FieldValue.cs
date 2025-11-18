using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class FieldValue
    {
        public int Id { get; set; }

        public int FormInstanceId { get; set; }
        [ForeignKey(nameof(FormInstanceId))]
        public FormInstance FormInstance { get; set; } = null!;

        public int FieldDefinitionId { get; set; }
        [ForeignKey(nameof(FieldDefinitionId))]
        public FieldDefinition FieldDefinition { get; set; } = null!;

        public string? FieldValue_ { get; set; } // Named with underscore to avoid conflict

        // For file/signature fields
        public byte[]? BinaryData { get; set; }

        // Metadata
        public int? CreatedById { get; set; }
        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        public int? ModifiedById { get; set; }
        [ForeignKey(nameof(ModifiedById))]
        public User? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [NotMapped]
        public string? Value
        {
            get => FieldValue_;
            set => FieldValue_ = value;
        }
    }
}
