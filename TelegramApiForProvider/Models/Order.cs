using System;
using System.Collections.Generic;

namespace TelegramApiForProvider.Models
{
    public class Order
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Наименование партнера
        /// </summary>
        public string PartnerName { get; set; }
        /// <summary>
        /// Идентификатор партнера
        /// </summary>
        public Guid PartnerId { get; set; }
        /// <summary>
        /// Номер заказа
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// Дата и время создания заказа
        /// </summary>
        public DateTime CreateDatetime { get; set; }
    }
}
