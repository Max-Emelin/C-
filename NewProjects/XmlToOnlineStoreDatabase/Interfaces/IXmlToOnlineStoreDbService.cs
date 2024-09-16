namespace XmlToOnlineStoreDb.Interfaces
{
    /// <summary>
    /// Для считывания XML-файла с информацией по заказам в онлайн-магазине.
    /// </summary>
    public interface IXmlToOnlineStoreDbService
    {
        /// <summary>
        /// Обработать XML-файла с информацией по заказам в онлайн-магазине.
        /// </summary>
        /// <param name="xmlFilePath"> Путь к файлу.</param>
        Task ProcessOrdersXmlAsync(string xmlFilePath);
    }
}