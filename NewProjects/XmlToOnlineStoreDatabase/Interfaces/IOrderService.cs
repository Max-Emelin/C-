namespace XmlToOnlineStoreDb.Interfaces
{
    /// <summary>
    /// Для заказов.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Записать в бд заказ.
        /// </summary>
        /// <param name="orderId"> Идентификатор заказа. </param>
        /// <param name="userId"> Идентификатор пользователя. </param>
        /// <param name="date"> Дата. </param>
        /// <param name="amount"> Общая сумма. </param>
        /// <returns></returns>
        Task InsertOrderAsync(int orderId, int userId, DateTime date, decimal amount);
    }
}