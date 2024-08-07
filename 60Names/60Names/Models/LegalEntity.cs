namespace _60Names.Models
{
    /// <summary>
    /// Юридическое лицо.
    /// </summary>
    public class LegalEntity
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование компании.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Инн.
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ОГРН.
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Страна.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        public string Phone { get; set; }
    }
}