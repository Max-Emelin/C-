namespace _60Names.Models
{
    /// <summary>
    /// Статусы договора.
    /// </summary>
    public enum ContractStatus
    {
        /// <summary>
        /// На обсуждении.
        /// </summary>
        UnderDiscussion,

        /// <summary>
        /// Не подтвержден.
        /// </summary>
        NotConcluded,

        /// <summary>
        /// Действует.
        /// </summary>
        InEffect,

        /// <summary>
        /// Выполнен.
        /// </summary>
        Executed,
        
        /// <summary>
        /// Приостановлен.
        /// </summary>
        Suspended,

        /// <summary>
        /// Расторгнут.
        /// </summary>
        Terminated
    }
}