namespace XmlToOnlineStoreDb.Interfaces
{
    /// <summary>
    /// Для пользователей.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Записать в бд пользователя.
        /// </summary>
        /// <param name="fullName"> ФИО. </param>
        /// <param name="email"> Почта. </param>
        /// <returns> Идентификатор пользователя. </returns>
        Task<int> InsertUserAsync(string fullName, string email);
    }
}