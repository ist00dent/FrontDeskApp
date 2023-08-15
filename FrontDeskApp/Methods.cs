using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data;

namespace FrontDeskApp
{
    internal class Methods
    {
        public static string conn = "";
        private static string customerTable = "tbl_Customers";
        private static string storedPackageTable = "tbl_Stored";

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }


        public static void addCustomer(CustomerService customer)
        {
            
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine().Trim();
            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine().Trim();
            Console.Write("Enter Phone Number: ");
            string phoneNumber;
            do
            {
                Console.Write("Enter phone number (digits only): ");
                phoneNumber = Console.ReadLine().Trim();
            } while (!IsDigitsOnly(phoneNumber));
            
            customer.AddCustomer(firstName, lastName, phoneNumber);
            Console.WriteLine("Customer added successfully!");
        }
        public static void checkAvailability(CustomerService customer)
        {
            
            Console.WriteLine("Enter box size (Small, Medium, Large): ");
            Console.WriteLine("(Enter B to Back)");
            customer.checkAvailability();
        }

        public static void storingPackage(CustomerService customer)
        {
            int customerID = 0, storageID = 1;
            string backNumber = "01";
            string package = "";
            Console.WriteLine("Enter 01 to back.");
            try
            {
                while (true)
                {
                    Console.Write("\nEnter Customer ID: ");
                    customerID = int.Parse(Console.ReadLine());
                    if(customerID.ToString() == backNumber)
                    {
                        return;
                    }
                    if (DatabaseManager.isDataExist(customerTable,"ID",customerID.ToString()))
                    {
                        break;
                    }
                    Console.WriteLine("ID not found.");
                }
            }
            catch { }
            try
            {
                while (true)
                {
                    Console.Write("Enter Package Name: ");
                    package = Console.ReadLine().Trim();
                    if (package == backNumber)
                    {
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(package))
                    {
                        break;
                    }
                }
            }
            catch { }
            try
            {
                while (true)
                {
                    Console.WriteLine("Enter Storage Area ID: ");
                    Console.WriteLine("1) Small");
                    Console.WriteLine("2) Medium");
                    Console.WriteLine("3) Large");
                    Console.Write(": ");
                    storageID = int.Parse(Console.ReadLine());
                    if (storageID.ToString() == backNumber)
                    {
                        return;
                    }
                    if (storageID > 3 || storageID < 1)
                    {
                        Console.WriteLine("Invalid storage area id");
                    }
                    else
                    {
                        if (DatabaseManager.CheckAvailability(storageID).Item2 > 0)
                            break;
                        else
                            Console.WriteLine("Not Enough Space.");
                    }
                }
            }
            catch { }

            customer.StorePackage(customerID, package, storageID);
        }

        public static void retrievingPackage(CustomerService customer)
        {
            int customerID = 0, storageID = 1;
            string package = "";
            string backNumber = "01";
            try
            {
                while (true)
                {
                    Console.Write("\nEnter Customer ID: ");
                    customerID = int.Parse(Console.ReadLine());
                    if (customerID.ToString() == backNumber)
                    {
                        return;
                    }
                    if (DatabaseManager.isDataExist(storedPackageTable, "[Customer ID]", customerID.ToString()))
                    {
                        break;
                    }
                    Console.WriteLine("ID does not have package");
                }
            }
            catch { }
            try
            {
                while (true)
                {
                    Console.Write("Enter Package Name: ");
                    package = Console.ReadLine().Trim();
                    if (package.ToString() == backNumber)
                    {
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(package))
                    {
                        Console.WriteLine("Invalid Package Name");
                    }
                    if (!DatabaseManager.isDataExist(storedPackageTable,"[Package Name]", package))
                    {
                        Console.WriteLine("Package Does not Exist");
                    }
                    if (!DatabaseManager.isDataExist(storedPackageTable,"[Customer ID]", customerID.ToString(),
                        "[Package Name]",package))
                    {
                        Console.WriteLine($"The ID {customerID} does not have package named {package}");
                    }
                    else
                    {
                        break;
                    }
                    
                }
            }
            catch { }
            try
            {
                while (true)
                {
                    Console.Write("Enter Storage Area ID: ");
                    storageID = int.Parse(Console.ReadLine());
                    if (storageID.ToString() == backNumber)
                    {
                        return;
                    }
                    if (storageID > 3 || storageID < 1)
                    {
                        Console.WriteLine("Invalid storage area id");
                    }
                    if (!DatabaseManager.isDataExist(storedPackageTable, "[Customer ID]", customerID.ToString(),
                        "[Package Name]", package,"[Storage Area ID]",storageID.ToString()))
                    {
                        Console.WriteLine($"The ID {customerID} does not have package named {package} " +
                            $"stored at area {storageID}");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch { }

            customer.RetrievePackage(customerID, package, storageID);
        }

        public static void updateCount()
        {
            DatabaseManager.updateStorageCount(1);
            DatabaseManager.updateStorageCount(2);
            DatabaseManager.updateStorageCount(3);
        }
    }
}
