using System;
using Newtonsoft.Json;

namespace AppCore
{
    public class Notification
    {
        public Notification()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
        
        [JsonProperty(PropertyName = "kind")]
        public NotificationKind Kind { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "showDialog")]
        public bool ShowDialog { get; set; }
    }
}
