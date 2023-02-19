using Microsoft.Data.SqlClient;
using ATM_DAL.Database;
using System;
using System.Threading.Tasks;

namespace ATM_BLL.Implementation
{
    public class CreateUsers : IDisposable
    {
        private readonly AtmDbContext _dbContext;
        private bool _disposed;

        public CreateUsers(AtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CreateUsers()
        {
        }

        public async Task CreateAtmUsers()
        {

            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string CreateUsers = @"CREATE TABLE ATM_Users (
                                    UserID INT PRIMARY KEY IDENTITY (1,1),
                                    
                                    FirstName VARCHAR(50) NOT NULL,
                                    LastName VARCHAR(50) NOT NULL,
                                    AccountName VARCHAR(max) NOT NULL,
                                    AccountNo VARCHAR(10) NOT NULL UNIQUE,
                                    AccountType VARCHAR(20) NOT NULL,
                                    Email VARCHAR(100) NOT NULL UNIQUE,
                                    PhoneNumber VARCHAR(14) NOT NULL UNIQUE,
                                    CardNumber VARCHAR(10) NOT NULL UNIQUE,
                                    Pin VARCHAR(4) NOT NULL UNIQUE,
                                    Balance DECIMAL(18, 2) NOT NULL
                                );
                                INSERT INTO ATM_Users (FirstName, LastName, AccountName, AccountNo, AccountType, 
                                                        Email, PhoneNumber, CardNumber, Pin, Balance)
                                VALUES ('Obi', 'Kenneth', 'Obi Kenneth', '8769006543', 'Savings', 
                                        'obikenneth@gmail.com', '09075666754', '1020304050', '1234', '5000.00'),
                                       ('Eze', 'Kings', 'Eze Kings', '7769006542', 'Savings', 
                                        'ezekings@gmail.com', '09075111754', '1121314151', '1030', '80000.00'),
                                       ('Okeke', 'Chinenye', 'Okeke Chinenye', '0430141954', 'Current', 
                                        'okekechinenye@gmail.com', '09071686750', '1222324052', '1935', '250000.00');";


            using SqlCommand command = new SqlCommand(CreateUsers, sqlConn);
            try
            {
                await command.ExecuteNonQueryAsync();
                

            }
            catch (Exception)
            {
                Console.Clear();
            }
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

