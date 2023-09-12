using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EManEmailMarketing.Storage.Models
{
    [Table("EmailCallback")]
    public class EmailCallback
    {
        [Key]
        public int EmailCallbackID { get; set; }
        public int SentEmailID { get; set; }

        public string? Event { get; set; }
        public DateTime Date { get; set; }


        public string? Email { get; set; }

        public string? IP { get; set; }
    }
}

