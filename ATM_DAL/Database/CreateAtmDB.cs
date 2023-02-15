using Microsoft.Data.SqlClient;
using System;

namespace ATM_DAL.Database
{
    public class CreateAtmDB 
    {
        public static void CreateDatabase()
        {

            string connectionString = (@"Data Source=(paste your server name here); Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

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
