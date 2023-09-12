using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EManEmailMarketing.Storage.Models
{
    [Table("ScrapedEmail")]
    public class ScrapedEmail
    {
        [Key]
        public int ScrapedEmailID { get; set; }
        public string? WebsiteLink { get; set; }
        public string? Email { get; set; }
        public string? KeyWordUsed { get; set; }
        public DateTime DateScraped { get; set; }
        public bool IsActive { get; set; }
        public bool? SendForPaul { get; set; }
    }
}
