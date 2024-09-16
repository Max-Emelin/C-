using System.Data.SqlClient;
using XmlToOnlineStoreDb.Interfaces;

namespace XmlToOnlineStoreDb.Services
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        public async Task<int> InsertUserAsync(string fullName, string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email) " +
                            "INSERT INTO Users (FullName, Email) VALUES (@FullName, @Email); " +
                            "SELECT UserId FROM Users WHERE Email = @Email;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@Email", email);

                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }
    }
}