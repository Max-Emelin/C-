using System.Data.SqlClient;
using XmlToOnlineStoreDb.Interfaces;

namespace XmlToOnlineStoreDb.Services
{
    /// <inheritdoc />
    public class OrderService : IOrderService
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        private readonly string _connectionString;

        public OrderService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task InsertOrderAsync(int orderId, int userId, DateTime date, decimal amount)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query =
                    "SET IDENTITY_INSERT Orders ON;" +
                    "DELETE FROM Orders WHERE OrderId = @OrderId;" +
                    "INSERT INTO Orders (OrderId, UserId, Date, Amount) " +
                    "VALUES (@OrderId, @UserID, @Date, @Amount);" +
                    "SET IDENTITY_INSERT Orders OFF;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Amount", amount);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
