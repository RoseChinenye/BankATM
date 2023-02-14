using ATM_DAL.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Transactions;

namespace ATM_DAL.Database
{
    public class CreateTransactionHistory 
    {
        private readonly AtmDbContext _dbContext;
        private bool _disposed;

        public CreateTransactionHistory(AtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public CreateTransactionHistory()
        {
        }

        public void TransactionHistory()
        {
            SqlConnection sqlConn = _dbContext.OpenConnection();

            string CreateTransactionTable = @"CREATE TABLE Transaction_History(
                                    TransactionID INT PRIMARY KEY IDENTITY(1, 1),
                                    UserCardNumber VARCHAR(10) NOT NULL,
                                    TransactionType VARCHAR(50) NOT NULL,
                                    Amount DECIMAL(18, 2) NOT NULL,
                                    Date datetime default CURRENT_TIMESTAMP,
                                    FOREIGN KEY(UserCardNumber) REFERENCES ATM_Users(CardNumber)
                                    )";
            
            using (SqlCommand command = new SqlCommand(CreateTransactionTable, sqlConn))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Transaction History table created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {

                    _dbContext.CloseConnection();
                }
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
