using System.Data;

namespace _60Names.Interfaces
{
    /// <summary>
    /// Сервис для договоров.
    /// </summary>
    public interface IContractService
    {
        /// <summary>
        /// Получение суммы всех заключенных договоров за текущий год.
        /// </summary>
        /// <returns> Сумма. </returns>
        decimal GetCurrentYearConcludedContractsAmount();

        /// <summary>
        /// Получение суммы заключенных договоров по каждому контрагенту из России.
        /// </summary>
        /// <returns> Контрагент, сумма. </returns>
        DataTable GetRussiaCounterpartyConcludedContractsAmount();

        /// <summary>
        /// Получение списка e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000.
        /// </summary>
        /// <returns> E-mail. </returns>
        List<string> GetEmailAuthorizedPersonsConcludedContractsLast30DaysAmountGreater40000();


        /// <summary>
        /// Изменение статуса договоров на "Расторгнут" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.
        /// </summary>
        /// <returns> Количество измененных договоров. </returns>
        int TerminateContractsIndividualsOver60();

        /// <summary>
        /// Создать отчет по физ. лицам, у которых есть действующие договора по компаниям, расположенных в городе Москва.
        /// </summary>
        void CreateReportIndividualsHaveValidContractsWithMoscowCompanies();

        /// <summary>
        /// Получение всех записей из бд.
        /// </summary>
        /// <returns> Все записи. </returns>
        string GetAllData();
    }
}