using System;

namespace TelegramApiForProvider.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public Guid PartnerId { get; set; }
        public long ChatId { get; set; }
    }
}
