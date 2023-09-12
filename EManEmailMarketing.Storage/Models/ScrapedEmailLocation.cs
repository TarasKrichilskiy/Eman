using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EManEmailMarketing.Storage.Models
{
    [Table("ScrapedEmailLocation")]
    public class ScrapedEmailLocation
    {
        [Key]
        public int ScrapedEmailLocationID { get; set; }
        public int ScrapedEmailID { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? ISP { get; set; }
        public bool? IsProxy { get; set; }
        public bool? IsHosted { get; set; }
        public bool? IsMobile { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
