namespace XmlToOnlineStoreDb.Interfaces
{
    /// <summary>
    /// Для товаров.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Записать в бд товар.
        /// </summary>
        /// <param name="name"> Название. </param>
        /// <param name="price"> Цена. </param>
        /// <returns> Идентификатор товара. </returns>
        Task<int> InsertProductAsync(string name, decimal price);
    }
}