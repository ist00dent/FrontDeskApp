using FrontDeskApp;
using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

class Program
{
    //for database directory
    private static string currentDirectory = System.IO.Directory.GetCurrentDirectory();
    private static string parentDirectory = Directory.GetParent(currentDirectory).FullName;
    public static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

    static DatabaseManager dbManager = new DatabaseManager();
    static CustomerService customerService = new CustomerService(dbManager);

    static void Main()
    {
        builder.DataSource = "(LocalDB)\\MSSQLLocalDB";
        builder.AttachDBFilename = $"{parentDirectory}\\Data\\StorageDB.mdf";
        builder.IntegratedSecurity = true;
        Methods.conn = builder.ConnectionString;
        Console.WriteLine("Storage Facility Front Desk App!\n");


        while (true)
        {
            Methods.updateCount();
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1) Add Customer");
            Console.WriteLine("2) Store Package");
            Console.WriteLine("3) Retrieve Package");
            Console.WriteLine("4) Check Available");
            Console.WriteLine("5) Exit\n");
            Console.Write(": ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Methods.addCustomer(customerService);
                    Thread.Sleep(1500);
                    Console.Clear();
                    break;

                case 2:
                    Methods.storingPackage(customerService);
                    Thread.Sleep(1500);
                    Console.Clear();
                    break;

                case 3:
                    Methods.retrievingPackage(customerService);
                    Thread.Sleep(1500);
                    Console.Clear();
                    break;

                case 4:
                    Methods.checkAvailability(customerService);
                    break;

                case 5:
                    Console.WriteLine("Exiting the application...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
        

}