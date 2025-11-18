using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class FieldDefinition
    {
        public int Id { get; set; }

        [Required]
        public int FormTemplateId { get; set; }

        [ForeignKey(nameof(FormTemplateId))]
        public FormTemplate FormTemplate { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FieldName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? FieldLabel { get; set; }

        [Required]
        [MaxLength(50)]
        public string FieldType { get; set; } = "Text"; // Text, Number, Date, Dropdown, Checkbox, Signature, Barcode

        // Position and Size
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        // Styling
        [MaxLength(100)]
        public string FontFamily { get; set; } = "Arial";
        public double FontSize { get; set; } = 12;

        [MaxLength(20)]
        public string FontColor { get; set; } = "#000000";

        [MaxLength(20)]
        public string? BackgroundColor { get; set; }

        [MaxLength(20)]
        public string? BorderColor { get; set; }

        public double BorderThickness { get; set; } = 0;

        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsUnderline { get; set; } = false;

        [MaxLength(20)]
        public string TextAlignment { get; set; } = "Left"; // Left, Center, Right

        [MaxLength(20)]
        public string VerticalAlignment { get; set; } = "Top"; // Top, Middle, Bottom

        // Validation
        public bool IsRequired { get; set; } = false;

        public string? DefaultValue { get; set; }

        [MaxLength(200)]
        public string? PlaceholderText { get; set; }

        [MaxLength(500)]
        public string? ValidationRegex { get; set; }

        [MaxLength(500)]
        public string? ValidationMessage { get; set; }

        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxValue { get; set; }

        // Dropdown/Checkbox Options (JSON array)
        public string? FieldOptions { get; set; }

        // Metadata
        public int TabIndex { get; set; } = 0;
        public bool IsReadOnly { get; set; } = false;
        public bool IsVisible { get; set; } = true;

        [MaxLength(500)]
        public string? HelpText { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
