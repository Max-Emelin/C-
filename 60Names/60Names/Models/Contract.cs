namespace _60Names.Models
{
    /// <summary>
    /// Договор.
    /// </summary>
    public class Contract
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор контрагента. 
        /// </summary>
        public int CounterpartyId { get; set; }

        /// <summary>
        /// Контрагент.
        /// </summary>
        public LegalEntity Counterparty { get; set; }

        /// <summary>
        /// Идентификатор уполномоченного лица. 
        /// </summary>
        public int AuthorizedPersonId { get; set; }

        /// <summary>
        /// Уполномоченное лицо.
        /// </summary>
        public Individual AuthorizedPerson { get; set; }

        /// <summary>
        /// Сумма договора.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        public ContractStatus Status { get; set; }

        /// <summary>
        /// Дата подписания.
        /// </summary>
        public DateTime SigningDate { get; set; }
    }
}