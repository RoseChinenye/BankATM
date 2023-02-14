using ATM_BLL.Interface;
using ATM_DAL.Database;
using ATM_DAL.Domain;
using Microsoft.Data.SqlClient;
using System;


namespace ATM_BLL.Implementation
{
    public class AuthServices : IAuthServices
    {

        private readonly AtmDbContext _dbContext;
        private bool _disposed;


        public AuthServices(AtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void UserLogin()
        {

            Console.WriteLine("Enter CardNumber: ");
            User.CardNumber = Console.ReadLine();

            Console.WriteLine("Enter Pin: ");
            User.Pin = Console.ReadLine();

            bool IsValid = false;

            string VerifyUser = "SELECT * FROM ATM_Users WHERE CardNumber = @CardNumber AND Pin = @Pin ";

            SqlConnection sqlConn = _dbContext.OpenConnection();

            using (SqlCommand command = new SqlCommand(VerifyUser, sqlConn))
            {
                try
                {
                    command.Parameters.AddWithValue("@CardNumber", User.CardNumber);
                    command.Parameters.AddWithValue("@Pin", User.Pin);


                    int result = (int)command.ExecuteScalar();


                    if (result > 0)
                    {
                        IsValid = true;
                    }


                }
                catch (Exception)
                {
                    Console.WriteLine("Login failed. Incorrect Card Number or Pin.\nPlease Enter Correct Card Number and Pin. ");
                }
                finally
                {

                    _dbContext.CloseConnection();
                }

                if (IsValid)
                {

                    Console.WriteLine("Login successful!\n\nPress ENTER to continue!");
                    Console.ReadKey();
                    Console.Clear();
                    AtmMenu.GetMenu();
                }
                else
                {
                    Console.WriteLine();
                    UserLogin();
                }

            }

        }


        public static void UserLogout()
        {
            Console.WriteLine("You have Successfully logged out!\nPress ENTER to Start again");
            Console.ReadKey();
            Console.Clear();
            starter.Run();
        }

        public void AdminLogin()
        {

        }

        public void AdminLogout()
        {

        }

        protected virtual void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
