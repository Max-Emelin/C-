using System.Globalization;
using System.Xml.Linq;
using XmlToOnlineStoreDatabase;
using XmlToOnlineStoreDb.Interfaces;

namespace XmlToOnlineStoreDb.Services
{
    /// <inheritdoc />
    public class XmlToOnlineStoreDbService : IXmlToOnlineStoreDbService
    {
        /// <summary>
        /// Сервис для работы с пользователями.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Сервис для работы с заказами.
        /// </summary>
        private readonly IOrderService _orderService;

        /// <summary>
        /// Сервис для работы с товарами.
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// Сервис для работы с товарами в заказе.
        /// </summary>
        private readonly IOrderProductService _orderProductService;

        /// <summary>
        /// Контекст базы данных.
        /// </summary>
        private readonly ApplicationDbContext _dbContext;

        public XmlToOnlineStoreDbService(IUserService userService, IOrderService orderService,
                            IProductService productService, IOrderProductService orderProductService,
                            ApplicationDbContext dbContext)
        {
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
            _orderProductService = orderProductService;
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task ProcessOrdersXmlAsync(string xmlFilePath)
        {
            var xmlDoc = XDocument.Load(xmlFilePath);

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var order in xmlDoc.Root.Elements("order"))
                    {
                        var fullName = order.Element("user").Element("fio").Value;
                        var email = order.Element("user").Element("email").Value;
                        var userId = await _userService.InsertUserAsync(fullName, email);

                        var orderId = int.Parse(order.Element("no").Value);
                        var date = DateTime.ParseExact(order.Element("reg_date").Value, "yyyy.MM.dd", null);
                        var amount = decimal.Parse(order.Element("sum").Value, CultureInfo.InvariantCulture);

                        await _orderService.InsertOrderAsync(orderId, userId, date, amount);

                        var products = order.Elements("product").ToList();

                        if (!order.Elements("product").Any())
                        {
                            throw new InvalidOperationException($"Заказ {orderId} без товаров.");
                        }

                        foreach (var product in products)
                        {
                            var name = product.Element("name").Value;
                            var price = decimal.Parse(product.Element("price").Value, CultureInfo.InvariantCulture);
                            var quantity = int.Parse(product.Element("quantity").Value);
                            var productId = await _productService.InsertProductAsync(name, price);

                            await _orderProductService.InsertOrderProductAsync(orderId, productId, quantity, price);
                        }
                    }

                    await transaction.CommitAsync();

                    Console.WriteLine("Данные успешно загружены в базу данных.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    Console.WriteLine($"Произошла ошибка: {ex.Message}.");
                }
            }
        }
    }
}