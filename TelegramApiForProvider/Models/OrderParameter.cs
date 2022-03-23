using System;
using System.Collections.Generic;
namespace TelegramApiForProvider.Models
{
    public class OrderParameter
    {
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя заказчика
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Наименование партнера
        /// </summary>
        public string PartnerName { get; set; }
        public Guid PartnerId { get; set; }
        /// <summary>
        /// Стоимость заказа
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Стоимость доставки
        /// </summary>
        public decimal DeliveryCost { get; set; }
        /// <summary>
        /// Значение скидки
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Номер телефона заказчика
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Номер заказа
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Дата и время создания заказа
        /// </summary>
        public DateTime CreateDatetime { get; set; }
        /// <summary>
        /// Адрес доставки
        /// </summary>
        public string DeliveryAddress { get; set; }
        /// <summary>
        /// Время к которому нужно доставить
        /// </summary>
        public TimeSpan? DeliverAtTime { get; set; }
        /// <summary>
        /// Содержимое заказа сериализованный в json
        /// </summary>
        public string OrderContent { get; set; }
        /// <summary>
        /// Количество столовых приборов
        /// </summary>
        public int CutleryQuantity { get; set; }
        /// <summary>
        /// Наличие у партнера доступа к админ панели
        /// </summary>
        public bool HasAdminPanel { get; set; }
        /// <summary>
        /// Сумма заказа с доставкой
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Тип оплаты
        /// </summary>
        public OrderPaymentMethods PaymentMethod { get; set; }
        /// <summary>
        /// Подробный адрес доставки
        /// </summary>
        public DeliveryLocation DeliveryLocation { get; set; }
        /// <summary>
        /// Тип доставки заказа
        /// </summary>
        public DeliveryType DeliveryType { get; set; }
        public List<OrderDish> Products { get; set; }
    }
}
