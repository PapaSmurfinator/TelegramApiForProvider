using System;
using System.Collections.Generic;

namespace TelegramApiForProvider.Models
{
    public class OrderDish
    {
        /// <summary>
        /// Идентификатор блюда в рамках корзины
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Идентификатор блюда
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Наименование блюда
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Цена порции
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// Ссылка на фото блюда
        /// </summary>
        public string PhotoUri { get; set; }
        /// <summary>
        /// Добавки
        /// </summary>
        public List<Additive> Additives { get; set; }
    }
}
