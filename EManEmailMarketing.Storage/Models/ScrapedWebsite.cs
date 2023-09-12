using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EManEmailMarketing.Storage.Models
{
    [Table("ScrapedWebsite")]
    public class ScrapedWebsite
    {
        [Key]
        public int ScrapedWebsiteID { get; set; }
        public string? Url { get; set; }
        public string? KeyWordUsed { get; set; }
        public bool HasBeenScraped { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
