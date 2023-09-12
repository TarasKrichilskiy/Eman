using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EManEmailMarketing.Storage.Models
{
    [Table("SentEmails")]
    public class SentEmail
    {
        [Key]
        public int SentEmailID { get; set; }
        public int ClientID { get; set; }
        public int? EmailID { get; set; }
        public string? FromRec { get; set; }
        public string? HTMLBody { get; set; }
        public DateTime SentDate { get; set; }
        public string? SendGridID { get; set; }
        public string? EmailCallback { get; set; }

        public DateTime EmailCallbackDate { get; set; }
        public string? ToEmail { get; set; }
        public int? ScrapedEmailID { get; set; }

        public Client Client { get; set; }
        public Emails Email { get; set; }
    }
}
