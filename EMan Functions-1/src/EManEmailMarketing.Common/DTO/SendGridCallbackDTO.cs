using Newtonsoft.Json;

namespace EManEmailMarketing.Common.DTO
{
    public class SendGridCallbackDTO
    {
        public string? Email { get; set; }
        public string? Event { get; set; }
        [JsonProperty("sg_message_id")]
        public string? EmailId { get; set; }
        public string? ip { get; set; }
    }
}
