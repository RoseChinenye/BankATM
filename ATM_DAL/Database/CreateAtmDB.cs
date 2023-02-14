using Microsoft.Data.SqlClient;
using System;

namespace ATM_DAL.Database
{
    public class CreateAtmDB
    {
        public static void CreateDatabase()
        {

            string connectionString = @"Data Source = LAPTOP-AI62M7MS\SQLEXPRESS; Integrated Security = True;";

            string createDBQuery = $"CREATE DATABASE AtmDatabase";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(createDBQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database created successfully.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating database: " + ex.Message);
                }
            }
            Console.ReadLine();

        }

    }
}