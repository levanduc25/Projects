using System;

namespace WEBSITE.Models
{
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime SendAt { get; set; }
        public bool IsFromAdmin { get; set; }
    }
}