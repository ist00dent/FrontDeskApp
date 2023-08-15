using System.Data.SqlClient;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client.Extensions.Msal;
using System.Data;

namespace FrontDeskApp
{
    internal class DatabaseManager
    {
        // Main Functions
        public void CreateCustomer(Customer customer)
        {
            using (SqlConnection conn = new SqlConnection(Methods.conn))
            {
                conn.Open();

                var sql = "INSERT INTO tbl_Customers VALUES (@ID, @FirstName, @LastName, @PhoneNumber)";

                using (var command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@ID", customer.Id);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static (int,int) CheckAvailability(int storageAreaID)
        {
            var availableSpace = (0,0);
            using (SqlConnection connection = new SqlConnection(Methods.conn))
            {
                connection.Open();

                string query = "SELECT Capacity,AvailableSpace FROM tbl_StorageAreas WHERE ID = '" +
                    storageAreaID + "'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            availableSpace = (reader.GetInt32(0), reader.GetInt32(1));

                        }
                    }
                    catch (Exception ex) { Console.Write(ex.Message); }
                }
            }
            return availableSpace;
        }

        public void RecordStorageEvent(BoxMovement boxEvent)
        {
            using (SqlConnection connection = new SqlConnection(Methods.conn))
            {
                connection.Open();
                string query = "INSERT INTO tbl_StorageEvents VALUES " +
                    "(@ID, @cID, @pCode, @saID, @time,@store)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", boxEvent.Id);
                    command.Parameters.AddWithValue("@cID", boxEvent.Id);
                    command.Parameters.AddWithValue("@pCode", boxEvent.PackageName);
                    command.Parameters.AddWithValue("@saID", boxEvent.StorageSize);
                    command.Parameters.AddWithValue("@time", boxEvent.Timestamp);
                    command.Parameters.AddWithValue("@store", boxEvent.IsStored);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex){ }
                }
            }
        }

        public void StoringPackage(BoxMovement boxEvent)
        {
            using (SqlConnection connection = new SqlConnection(Methods.conn))
            {
                connection.Open();
                string query = "INSERT INTO tbl_Stored VALUES (@ID, @cID, @pCode, @saID)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", boxEvent.Id);
                    command.Parameters.AddWithValue("@cID", boxEvent.CustomerId);
                    command.Parameters.AddWithValue("@pCode", boxEvent.PackageName);
                    command.Parameters.AddWithValue("@saID", boxEvent.StorageSize);
                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Stored Successfully");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
        }



        public void RetrievingPackage(BoxMovement boxEvent)
        {
            using (SqlConnection connection = new SqlConnection(Methods.conn))
            {
                connection.Open();
                string query = "DELETE FROM tbl_Stored WHERE [Customer ID] = '" + boxEvent.CustomerId +
                    "' AND [Package Name] = '" + boxEvent.PackageName + "' AND [Storage Area ID] = '" + 
                    boxEvent.StorageSize + "'";

                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Retrieved Successfully");
                }
            }
        }

        // Important methods
        public bool isPackageExists(int CustomerID, int StorageID, string packageCode)
        {
            bool found = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Methods.conn))
                {
                    conn.Open();
                    string selectStatement = "SELECT ID FROM tbl_Stored WHERE [Package Code] = '" + packageCode + "' " +
                        ", [Customer ID] = '" + CustomerID + "' AND [Storage Area ID] = '" + StorageID + "'";
                    using (SqlCommand command = new SqlCommand(selectStatement, conn))
                    {
                        int count = (int)command.ExecuteScalar();
                        found = count > 0 ? true : false;
                    }
                }
                return found;
            }
            catch (Exception ex)
            {
                return found;
            }
        }

        public static bool isDataExist(string tableName, string column, string value)
        {
            bool found = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Methods.conn))
                {
                    conn.Open();
                    string query = "IF EXISTS (SELECT 1 FROM " + tableName + " WHERE " + column + " = '" + value + "') " +
                        "SELECT 1 ELSE SELECT 0";
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            int result = Convert.ToInt32(command.ExecuteScalar());
                            found = result > 0 ? true : false;
                        }
                    }
                    catch (Exception ex) {  }
                }
                return found;
            }
            catch (Exception ex)
            {
                return found;
            }
        }
        public static bool isDataExist(string tableName, string column, string value, string column2, 
            string value2)
        {
            bool found = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Methods.conn))
                {
                    conn.Open();
                    string query = "IF EXISTS (SELECT 1 FROM " + tableName + " WHERE " + column + " = '" + value + "'" +
                        "AND " + column2 + "='" + value2 + "') SELECT 1 ELSE SELECT 0";
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            int result = Convert.ToInt32(command.ExecuteScalar());
                            found = result > 0 ? true : false;
                        }
                    }
                    catch (Exception ex) { }
                }
                return found;
            }
            catch (Exception ex)
            {
                return found;
            }
        }
        public static bool isDataExist(string tableName, string column, string value, string column2,
            string value2, string column3, string value3)
        {
            bool found = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Methods.conn))
                {
                    conn.Open();
                    string query = "IF EXISTS (SELECT 1 FROM " + tableName + " WHERE " + column + " = '" + value + "'" +
                        "AND " + column2 + "='" + value2 + "' AND " + column3 + "='" + value3 + "') SELECT 1 ELSE SELECT 0";
                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, conn))
                        {
                            int result = Convert.ToInt32(command.ExecuteScalar());
                            found = result > 0 ? true : false;
                        }
                    }
                    catch (Exception ex) { }
                }
                return found;
            }
            catch (Exception ex)
            {
                return found;
            }
        }

        private static void storageMovement(int storageID, int newNum)
        {
            using (SqlConnection connection = new SqlConnection(Methods.conn))
            {
                connection.Open();
                string query = "UPDATE tbl_StorageAreas SET AvailableSpace = '" + newNum +
                    "' WHERE ID = '" + storageID + "'";

                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void updateStorageCount(int storageAreaID) 
        {
            var newNum = CheckAvailability(storageAreaID);
            int rem = newNum.Item1 - dataCounter("tbl_Stored","[Storage Area ID]",storageAreaID.ToString());
            storageMovement(storageAreaID, rem);
            
        }

        public static int dataCounter(string tableName, string columnName, string find)
        {
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(Methods.conn))
                {
                    conn.Open();
                    string selectStatement = "SELECT COUNT(*) FROM " +
                        tableName.Replace("'", "''") + " WHERE " + columnName.Replace("'", "''") + " = '" + find.Replace("'", "''") + "'";

                    using (SqlCommand command = new SqlCommand(selectStatement, conn))
                    {
                        count = (int)command.ExecuteScalar();
                        return count;
                    }
                }
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        public static int getNextID(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(Methods.conn))
            {
                conn.Open();
                int ctr = 1;
                string check = "";
                int tempcount;

                while (true)
                {
                    check = "SELECT COUNT (ID) FROM " + tableName.Replace("'", "''") + " WHERE ID = '" + ctr + "'";
                    SqlCommand cmd = new SqlCommand(check, conn);
                    tempcount = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                    if (tempcount != 0)
                    {
                        ctr++;
                    }
                    else
                    {
                        return ctr;
                    }
                }
            }


        }

    }
}
