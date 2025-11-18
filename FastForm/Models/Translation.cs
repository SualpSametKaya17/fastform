using System.ComponentModel.DataAnnotations;

namespace FastForm.Models
{
    public class Translation
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string LanguageCode { get; set; } = "en"; // en, tr, de, etc.

        [Required]
        [MaxLength(200)]
        public string ResourceKey { get; set; } = string.Empty;

        [Required]
        public string ResourceValue { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Category { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
