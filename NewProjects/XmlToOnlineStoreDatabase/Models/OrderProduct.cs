namespace XmlToOnlineStoreDatabase.Models
{
    /// <summary>
    /// Товар в заказе.
    /// </summary>
    public class OrderProduct
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int OrderProductId { get; set; }

        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Идентификатор товара.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Количество товара в заказе.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Цена на момент заказа.
        /// </summary>
        public decimal PriceAtOrder { get; set; }
    }
}