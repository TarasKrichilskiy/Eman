using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EManEmailMarketing.Storage.Models
{
    [Table("ClientProgress")]
    public class ClientProgress
    {
        [Key]
        public int ClientProgressID { get; set; }

        public int ClientID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Opens { get; set; }
        public int Clicks { get; set; }
        public int Sends { get; set; }
        public int OpensGoal { get; set; }
        public int ClicksGoal { get; set; }
        public int SendsGoal { get; set; }
        public string? EmailHTMLTemplate { get; set; }
        public string? EmailSubject { get; set; }
        public int IsActive { get; set; }
    }
}
