using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EManEmailMarketing.Storage.Models
{
    [Table("Emails")]
    public class Emails
    {
        [Key]
        public int EmailID { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Title { get; set; }
        public Double? LeadNumber { get; set; }
        public string? Source { get; set; }
        public int? EmailQualityID { get; set; }
        public bool? SendForPaul { get; set; }
        public bool? SendForPSGcontractors { get; set; }
        public ICollection<SentEmail> SentEmails { get; set; }
    }
}
