using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XmlToOnlineStoreDatabase;
using XmlToOnlineStoreDb.Interfaces;
using XmlToOnlineStoreDb.Services;

start().Wait();

async Task start()
{
    var connectionToOnlineStoreDb = "Server=(localdb)\\mssqllocaldb;Database=OnlineStore;Trusted_Connection=True;";
    var xmlFilePath = "C:\\Users\\I\\Desktop\\orders.xml";
    var serviceProvider = new ServiceCollection()
        .AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionToOnlineStoreDb))
        .AddTransient<IUserService>(s => new UserService(connectionToOnlineStoreDb))
        .AddTransient<IOrderService>(s => new OrderService(connectionToOnlineStoreDb))
        .AddTransient<IProductService>(s => new ProductService(connectionToOnlineStoreDb))
        .AddTransient<IOrderProductService>(s => new OrderProductService(connectionToOnlineStoreDb))
        .AddTransient<IXmlToOnlineStoreDbService, XmlToOnlineStoreDbService>()
        .BuildServiceProvider();
    var xmlToOnlineStoreDbService = serviceProvider.GetService<IXmlToOnlineStoreDbService>();

    await xmlToOnlineStoreDbService.ProcessOrdersXmlAsync(xmlFilePath);
}