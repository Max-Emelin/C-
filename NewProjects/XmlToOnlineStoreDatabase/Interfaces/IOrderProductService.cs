namespace XmlToOnlineStoreDb.Interfaces
{
    /// <summary>
    /// Для товаров в заказе.
    /// </summary>
    public interface IOrderProductService
    {
        /// <summary>
        /// Записать в бд товар в заказе.
        /// </summary>
        /// <param name="orderId"> Идентификатор заказа. </param>
        /// <param name="productId">Идентификатор продукта. </param>
        /// <param name="quantity"> Количество. </param>
        /// <param name="priceAtOrder"> Цена в момент заказа.</param>
        Task InsertOrderProductAsync(int orderId, int productId, int quantity, decimal priceAtOrder);
    }
}