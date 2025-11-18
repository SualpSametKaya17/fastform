using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastForm.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; } = string.Empty;

        public string? SettingValue { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? DataType { get; set; } // String, Int, Bool, Json

        public bool IsSystem { get; set; } = false;

        public int? ModifiedById { get; set; }
        [ForeignKey(nameof(ModifiedById))]
        public User? ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
