using _60Names;
using _60Names.Interfaces;
using _60Names.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

var defaultConnection = "Server=(localdb)\\mssqllocaldb;Database=_60Names;Trusted_Connection=True;";

Start();

void Start()
{
    var serviceProvider = MakeServiceProvider();

    using (var scope = serviceProvider.CreateScope())
    {
        var contractService = scope.ServiceProvider.GetRequiredService<IContractService>();
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Главное меню:");

            Console.WriteLine("1. Вывести сумму всех заключенных договоров за текущий год.");
            Console.WriteLine("2. Вывести сумму заключенных договоров по каждому контрагенту из России.");
            Console.WriteLine("3. Вывести список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000.");
            Console.WriteLine("4. Изменить статус договора на \"Расторгнут\" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.");
            Console.WriteLine("5. Создать отчет по физ. лицам, у которых есть действующие договора по компаниям, расположенных в городе Москва.");
            Console.WriteLine("6. Показать все записи");
            Console.WriteLine("7. Выход");

            Console.Write("\nВыберите опцию (1-7): ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Task1(contractService);
                    break;
                case "2":
                    Task2(contractService);
                    break;
                case "3":
                    Task3(contractService);
                    break;
                case "4":
                    Task4(contractService);
                    break;
                case "5":
                    Task5(contractService);
                    break;
                case "6":
                    Console.WriteLine(contractService.GetAllData());
                    break;
                case "7":
                    running = false;
                    Console.WriteLine("Выход из программы...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }

            if (input != "7")
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}

void Task1(IContractService contractService)
{
    Console.WriteLine($"1) Вывести сумму всех заключенных договоров за текущий год: {contractService.GetCurrentYearConcludedContractsAmount()}");
}

void Task2(IContractService contractService)
{
    Console.WriteLine($"\n2) Вывести сумму заключенных договоров по каждому контрагенту из России:");

    foreach (DataRow row in contractService.GetRussiaCounterpartyConcludedContractsAmount().Rows)
    {
        var companyName = row["CompanyName"].ToString();
        var totalAmount = Convert.ToDecimal(row["TotalAmmount"]);

        Console.WriteLine($"CompanyName: {companyName}, Total Amount: {totalAmount}");
    }
}

void Task3(IContractService contractService)
{
    Console.WriteLine($"\n3) Вывести список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000:");

    foreach (var email in contractService.GetEmailAuthorizedPersonsConcludedContractsLast30DaysAmountGreater40000())
    {
        Console.WriteLine(email);
    }
}

void Task4(IContractService contractService)
{
    Console.WriteLine($"\n4) Статус договора был обновлен для {contractService.TerminateContractsIndividualsOver60()} записей:");
}

void Task5(IContractService contractService)
{
    Console.WriteLine($"\n5) Создать отчет по физ. лицам, у которых есть действующие договора по компаниям, расположенных в городе Москва:");

    contractService.CreateReportIndividualsHaveValidContractsWithMoscowCompanies();

    Console.WriteLine("Отчет создан, файл : report.json");
}

ServiceProvider MakeServiceProvider()
{
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(defaultConnection));
    serviceCollection.AddTransient<IContractService, ContractService>();

    return serviceCollection.BuildServiceProvider();
}