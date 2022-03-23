using System;

namespace TelegramApiForProvider.Models
{
    public class DeliveryLocation
    {
        /// <summary>
        /// Наименование города
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Адрес доставки (улица + дом)
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// Номер дома
        /// </summary>
        public string Home { get; set; }
        /// <summary>
        /// Подъезд
        /// </summary>
        public string Entrance { get; set; }
        /// <summary>
        /// Этаж
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// Квартира
        /// </summary>
        public string Apartment { get; set; }
    }
}
