using _60Names.Interfaces;
using _60Names.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Text.Json;

namespace _60Names.Services
{
    /// <inheritdoc />
    public class ContractService : IContractService
    {
        /// <summary>
        /// Контекст бд.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Строка подключения к бд.
        /// </summary>
        private readonly string defaultConnection = "Server=(localdb)\\mssqllocaldb;Database=_60Names;Trusted_Connection=True;";

        public ContractService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public decimal GetCurrentYearConcludedContractsAmount()
        {
            using (var connection = new SqlConnection(defaultConnection))
            {
                var sqlQuery = @"SELECT SUM(Amount)
                    FROM Contracts
                    WHERE YEAR(SigningDate) = @NowYear";
                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@NowYear", DateTime.Now.Year);
                connection.Open();

                return (decimal)command.ExecuteScalar();
            }
        }

        /// <inheritdoc />
        public List<string> GetEmailAuthorizedPersonsConcludedContractsLast30DaysAmountGreater40000()
        {
            var emailList = new List<string>();

            using (var connection = new SqlConnection(defaultConnection))
            {
                var sqlQuery = @"
                    SELECT DISTINCT Email
                    FROM Contracts 
                    LEFT JOIN Individuals  ON Contracts.AuthorizedPersonId = Individuals.Id
                    WHERE SigningDate >= DATEADD(DAY, -30, GETDATE())
                    AND SigningDate < CAST(GETDATE() AS DATE)
                    AND Amount > 40000";
                var command = new SqlCommand(sqlQuery, connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    string email = row["Email"].ToString();
                    emailList.Add(email);
                }

                return emailList;
            }
        }

        /// <inheritdoc />
        public DataTable GetRussiaCounterpartyConcludedContractsAmount()
        {
            using (var connection = new SqlConnection(defaultConnection))
            {
                var sqlQuery = @"SELECT CompanyName, SUM(Amount) AS TotalAmmount 
                    FROM Contracts 
                    LEFT JOIN LegalEntities ON Contracts.CounterpartyId = LegalEntities.Id
                    WHERE Country = 'Russia'
                    GROUP BY CompanyName";
                var command = new SqlCommand(sqlQuery, connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();

                connection.Open();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        /// <inheritdoc />
        public int TerminateContractsIndividualsOver60()
        {
            using (var connection = new SqlConnection(defaultConnection))
            {
                var sqlQuery = @"
                    UPDATE Contracts
                    SET Status = @Terminated
                    WHERE Status <> @Terminated
                    AND AuthorizedPersonId IN (
                        SELECT Id
                        FROM Individuals 
                        WHERE Birthday <= DATEADD(YEAR, -60, GETDATE())
                        );
                    ";
                var command = new SqlCommand(sqlQuery, connection);

                command.Parameters.AddWithValue("@Terminated", ContractStatus.Terminated);
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        /// <inheritdoc />
        public void CreateReportIndividualsHaveValidContractsWithMoscowCompanies()
        {
            var individuals = _context.Individuals
                .Join(_context.Contracts,
                      i => i.Id,
                      c => c.AuthorizedPersonId,
                      (i, c) => new { Individual = i, Contract = c })
                .Join(_context.LegalEntities,
                      i => i.Contract.CounterpartyId,
                      l => l.Id,
                      (i, l) => new { i.Individual, i.Contract, LegalEntity = l })
                .Where(x => x.Contract.Status == ContractStatus.InEffect && x.LegalEntity.City == "Moscow")
                .Select(x => new
                {
                    x.Individual.Name,
                    x.Individual.Surname,
                    x.Individual.Patronymic,
                    x.Individual.Email,
                    x.Individual.Phone,
                    DateOfBirth = x.Individual.Birthday.ToString("yyyy-MM-dd")
                })
                .ToList();

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(individuals, jsonOptions);

            File.WriteAllText("report.json", json);
        }

        /// <inheritdoc />
        public string GetAllData()
        {
            var individuals = _context.Individuals.ToList();
            var legalEntities = _context.LegalEntities.ToList();
            var contracts = _context.Contracts.ToList();
            var data = new StringBuilder("Получение информации:");

            data.AppendLine("\nСписок Контрагент :");

            foreach (var e in legalEntities)
            {
                data.AppendLine($"Id: {e.Id}, CompanyName: {e.CompanyName}, INN: {e.INN},Country:  {e.Country}");
            }

            data.AppendLine("\nСписок Уполномоченное лицо :");

            foreach (var e in individuals)
            {
                data.AppendLine($"Id: {e.Id}, Name: {e.Name} {e.Surname}");
            }

            data.AppendLine("\nСписок Договор :");

            foreach (var e in contracts)
            {
                data.AppendLine($"Id: {e.Id}, " +
                    $"Counterparty: {e.Counterparty.CompanyName}, " +
                    $"AuthorizedPerson:  {e.AuthorizedPerson.Name}, " +
                    $"Amount:  {e.Amount}, " +
                    $"Date: {e.SigningDate}");
            }

            return data.ToString();
        }
    }
}