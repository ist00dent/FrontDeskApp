using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDeskApp
{
    internal class CustomerService
    {
        private DatabaseManager _dbManager;

        public CustomerService(DatabaseManager dbManager)
        {
            _dbManager = dbManager;
        }


        public void AddCustomer(string firstName, string lastName, string phoneNumber)
        {
            var customer = new Customer
            {
                Id = DatabaseManager.getNextID("tbl_Customers"),
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };

            _dbManager.CreateCustomer(customer);
        }

        public void StorePackage(int customerId, string packagecode, int storageAreaId)
        {
            var boxEvent = new BoxMovement
            {
                Id = DatabaseManager.getNextID("tbl_Stored"),
                CustomerId = customerId,
                PackageName = packagecode,
                StorageSize = storageAreaId,
                Timestamp = DateTime.Now,
                IsStored = "Y"
            };
            _dbManager.StoringPackage(boxEvent);
            RecordStorageEvent(boxEvent);
        }

        public void RetrievePackage(int customerId, string packagecode, int storageAreaId)
        {
            var boxEvent = new BoxMovement
            {
                CustomerId = customerId,
                PackageName = packagecode,
                StorageSize = storageAreaId,
                Timestamp = DateTime.Now,
                IsStored = "N"
            };
            _dbManager.RetrievingPackage(boxEvent);
            RecordStorageEvent(boxEvent);
        }

        public void RecordStorageEvent(BoxMovement boxEvent)
        {
            boxEvent.Id = DatabaseManager.getNextID("tbl_StorageEvents");
            _dbManager.RecordStorageEvent(boxEvent);
        }

        public void checkAvailability()
        {
            string response;
            while (true)
            {
                Console.Write("\n: ");
                try
                {
                    response = Console.ReadLine();
                    if (response.ToUpper() == "B")
                    {
                        break;
                    }
                    if (Enum.TryParse(response.ToUpper(), out BoxSize size))
                    {
                        switch (response.ToUpper())
                        {
                            case "SMALL":
                                Console.WriteLine("Available Small Packages Area: " + DatabaseManager.CheckAvailability(1).Item2);
                                break;
                            case "MEDIUM":
                                Console.WriteLine("Available Medium Packages Area: " + DatabaseManager.CheckAvailability(2).Item2);
                                break;
                            case "LARGE":
                                Console.WriteLine("Available Large Packages Area: " + DatabaseManager.CheckAvailability(3).Item2);
                                break;
                            default:
                                Console.WriteLine("Invalid Size");
                                break;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid box size.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        


    }
}
