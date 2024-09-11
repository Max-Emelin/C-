namespace XmlToOnlineStoreDatabase.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Полное имя.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; }
    }
}