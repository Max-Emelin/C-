namespace XmlToOnlineStoreDatabase.Models
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Время заказа.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Общая стоимость заказа.
        /// </summary>
        public decimal Amount { get; set; }
    }
}