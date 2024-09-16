using System.Data.SqlClient;
using XmlToOnlineStoreDb.Interfaces;

namespace XmlToOnlineStoreDb.Services
{
    /// <inheritdoc />
    public class ProductService : IProductService
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        private readonly string _connectionString;

        public ProductService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<int> InsertProductAsync(string name, decimal price)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query =
                   "IF NOT EXISTS " +
                   "   (SELECT 1 " +
                   "   FROM Products " +
                   "   WHERE Name = @Name) " +
                   "INSERT INTO Products (Name, Price) " +
                   "VALUES (@Name, @Price); " +
                   "SELECT ProductId " +
                   "FROM Products " +
                   "WHERE Name = @Name;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);

                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }
    }
}
