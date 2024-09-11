using System.Data.SqlClient;
using System.Globalization;
using System.Xml.Linq;
using XmlToOnlineStoreDatabase;


start();


static void start()
{
    using (var context = new ApplicationDbContext())
    {
        context.Database.EnsureCreated();
    }

    XmlToDB().Wait();
}


static async Task XmlToDB()
{
    string xmlFilePath = "C:\\Users\\I\\Desktop\\orders.xml";
    string connectionString = "Server=(localdb)\\mssqllocaldb;Database=OnlineStore;Trusted_Connection=True;";

    try
    {
        var xmlDoc = XDocument.Load(xmlFilePath);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            foreach (var order in xmlDoc.Root.Elements("order"))
            {
                var fullName = order.Element("user").Element("fio").Value;
                var email = order.Element("user").Element("email").Value;
                var userId = await UsersInsert(connection, fullName, email);
                var orderId = int.Parse(order.Element("no").Value);
                var date = DateTime.ParseExact(order.Element("reg_date").Value, "yyyy.MM.dd", null);
                var amount = decimal.Parse(order.Element("sum").Value, CultureInfo.InvariantCulture);

                await OrdersInsert(connection, orderId, userId, date, amount);

                foreach (var product in order.Elements("product"))
                {
                    var name = product.Element("name").Value;
                    var price = decimal.Parse(product.Element("price").Value, CultureInfo.InvariantCulture);
                    var quantity = int.Parse(product.Element("quantity").Value);
                    var productId = await ProductsInsert(connection, name, price);

                    await OrderProductsInsert(connection, orderId, productId, quantity, price);
                }
            }
            Console.WriteLine("Данные успешно загружены в базу данных.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Произошла ошибка: {ex.Message}");
    }
}



static async Task OrderProductsInsert(SqlConnection connection, int orderId, int productId, int quantity, decimal priceAtOrder)
{
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

static async Task<int> ProductsInsert(SqlConnection connection, string name, decimal price)
{
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

static async Task OrdersInsert(SqlConnection connection, int orderId, int userId, DateTime date, decimal amount)
{
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

static async Task<int> UsersInsert(SqlConnection connection, string fullName, string email)
{
    var query =
        "IF NOT EXISTS " +
            "(SELECT 1 " +
            "FROM Users " +
            "WHERE Email = @Email) " +
        "INSERT INTO Users (FullName, Email) " +
        "VALUES (@FullName, @Email); " +
        "SELECT UserId " +
        "FROM Users " +
        "WHERE Email = @Email;";

    using (SqlCommand command = new SqlCommand(query, connection))
    {
        command.Parameters.AddWithValue("@FullName", fullName);
        command.Parameters.AddWithValue("@Email", email);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }
}