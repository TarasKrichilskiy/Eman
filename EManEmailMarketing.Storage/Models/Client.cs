using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EManEmailMarketing.Storage.Models
{
    [Table("Clients")]
    public class Client
    {
        [Key]
        public int ClientID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? PlanStartDate { get; set; }

        public DateTime? PlanEndDate { get; set; }

        public int PlanClicksPerMonth { get; set; }

        public int PlanOpensPerMonth { get; set; }

        public decimal? PlanMonthlyCost { get; set; }

        public bool IsActive { get; set; }

        public string? BusinessName { get; set; }

        public string? BusinessWebsite { get; set; }
        public string? StateFilter { get; set; }
        public ICollection<SentEmail> SentEmailList { get; set; }
    }
}

