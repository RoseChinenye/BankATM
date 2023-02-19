using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace ATM_DAL.Database
{
    public class CreateAtmDB 
    {
       
        public static async Task CreateDatabase()
        {

            string connectionString = (@"Data Source=LAPTOP-AI62M7MS\SQLEXPRESS; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");


            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            try
            {
                string createDBQuery = $"CREATE DATABASE AtmDatabase";

                SqlCommand command = new SqlCommand(createDBQuery, connection);

                await command.ExecuteNonQueryAsync();
                

            }
            catch (Exception)
            {
                Console.Clear();

            }


        }

    }
}