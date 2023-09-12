using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EManEmailMarketing.Storage.Models
{
    [Table("KeyWord")]
    public class KeyWord
    {
        [Key]
        public int KeyWordID { get; set; }

        public string? Word { get; set; }

        public bool? IsActive { get; set; }
    }
}

