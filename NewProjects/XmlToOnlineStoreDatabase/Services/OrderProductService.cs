using System.Data.SqlClient;
using XmlToOnlineStoreDb.Interfaces;

namespace XmlToOnlineStoreDb.Services
{
    /// <inheritdoc />
    public class OrderProductService : IOrderProductService
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        private readonly string _connectionString;

        public OrderProductService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task InsertOrderProductAsync(int orderId, int productId, int quantity, decimal priceAtOrder)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO OrderProducts (OrderId, ProductId, Quantity, PriceAtOrder) " +
                          "VALUES (@OrderId, @ProductId, @Quantity, @PriceAtOrder)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@PriceAtOrder", priceAtOrder);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}