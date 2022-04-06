using System;

namespace TelegramApiForProvider.Models
{
    public class OrderMessage
    {
        public Guid Id { get; set; }
        public bool? IsAccept { get; set; }
        public int? MessageId { get; set; }
        public long ChatId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
