using ATM_BLL.Interface;
using ATM_DAL.Database;
using ATM_DAL.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;


namespace ATM_BLL.Implementation
{
    public class AtmServices : IAtmServices
    {
        private readonly AtmDbContext _dbContext;
        private bool _disposed;

        public AtmServices(AtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Deposit()
        {

            Console.WriteLine("Enter deposit amount: ");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || input == "0" || !decimal.TryParse(input, out _))
            {
                Console.WriteLine("Invalid input. Please enter a valid deposit amount: ");
                input = Console.ReadLine();
            }
            decimal depositAmount = Convert.ToDecimal(input);


            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string BalanceQuery = "SELECT Balance FROM ATM_Users WHERE CardNumber = @cardNumber AND PIN = @pin";

            await using SqlCommand command = new SqlCommand(BalanceQuery, sqlConn);
            try
            {
                command.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                command.Parameters.AddWithValue("@pin", User.Pin);

                decimal Balance = (decimal)await command.ExecuteScalarAsync();


                Balance += depositAmount;

                string TransactionType = "Deposit";
                DateTime TransactionTime = DateTime.Now;

                string updateQuery = "UPDATE ATM_Users SET Balance = @balance WHERE CardNumber = @cardNumber AND Pin = @pin";
                await using (SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConn))
                {
                    updateCommand.Parameters.AddWithValue("@balance", Balance);
                    updateCommand.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                    updateCommand.Parameters.AddWithValue("@pin", User.Pin);
                    await updateCommand.ExecuteNonQueryAsync();
                }


                string InsertHistoryQuery = @"INSERT INTO Transaction_History (UserCardNumber, TransactionType, Amount, Date)
                                                   VALUES (@UserCardNumber, @TransactionType, @Amount, @Date)";

                await using (SqlCommand historyCommand = new SqlCommand(InsertHistoryQuery, sqlConn))
                {
                    historyCommand.Parameters.AddWithValue("@UserCardNumber", User.CardNumber);
                    historyCommand.Parameters.AddWithValue("@TransactionType", TransactionType);
                    historyCommand.Parameters.AddWithValue("@Amount", depositAmount);
                    historyCommand.Parameters.AddWithValue("@Date", TransactionTime);

                    await historyCommand.ExecuteNonQueryAsync();
                }


                Console.WriteLine($"Deposit Successful! Your new balance is: NGN {Balance}\n\nPress ENTER to go to Main Menu");

                Console.ReadKey();
                Console.Clear();
                await AtmMenu.GetMenu();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            
        }
        public async Task Withdraw()
        {
            
            Console.WriteLine("Enter amount you want to withdraw: ");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || input == "0" || !decimal.TryParse(input, out _))
            {
                Console.WriteLine("Invalid input. Please enter a valid amount: ");
                input = Console.ReadLine();
            }
            decimal WithdrawalAmount = Convert.ToDecimal(input);


            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string BalanceQuery = "SELECT Balance FROM ATM_Users WHERE CardNumber = @cardNumber AND PIN = @pin";


            using SqlTransaction sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                using SqlCommand command = new SqlCommand(BalanceQuery, sqlConn, sqlTransaction);
                try
                {
                    command.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                    command.Parameters.AddWithValue("@pin", User.Pin);

                    decimal Balance = (decimal)await command.ExecuteScalarAsync();


                    if (Balance < WithdrawalAmount)
                    {
                        Console.WriteLine("Insufficient balance. Your current balance is: " + Balance);
                        await Withdraw();
                    }
                    else
                    {
                        Balance -= WithdrawalAmount;


                        string TransactionType = "Withdrawal";
                        DateTime TransactionTime = DateTime.Now;

                        string updateQuery = "UPDATE ATM_Users SET Balance = @balance WHERE CardNumber = @cardNumber AND Pin = @pin";
                        await using (SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConn, sqlTransaction))
                        {
                            updateCommand.Parameters.AddWithValue("@balance", Balance);
                            updateCommand.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                            updateCommand.Parameters.AddWithValue("@pin", User.Pin);
                            await updateCommand.ExecuteNonQueryAsync();
                        }


                        string InsertHistoryQuery = @"INSERT INTO Transaction_History (UserCardNumber, TransactionType, Amount, Date)
                                                   VALUES (@UserCardNumber, @TransactionType, @Amount, @Date)";

                        await using (SqlCommand historyCommand = new SqlCommand(InsertHistoryQuery, sqlConn, sqlTransaction))
                        {
                            historyCommand.Parameters.AddWithValue("@UserCardNumber", User.CardNumber);
                            historyCommand.Parameters.AddWithValue("@TransactionType", TransactionType);
                            historyCommand.Parameters.AddWithValue("@Amount", WithdrawalAmount);
                            historyCommand.Parameters.AddWithValue("@Date", TransactionTime);

                            await historyCommand.ExecuteNonQueryAsync();
                        }

                        Console.WriteLine($"Withdrawal successful. Your new balance is: #{Balance}\n\nPress ENTER to go to Main Menu");

                        Console.ReadKey();
                        Console.Clear();
                        await AtmMenu.GetMenu();

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e);
                }
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
            }
        }

        public async Task Transfer()
        {
            
            Console.WriteLine("Enter Reciever Account Number: ");
            string input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || input == "0" || !decimal.TryParse(input, out _))
            {
                Console.WriteLine("Invalid input. Please enter a valid input: ");
                input = Console.ReadLine();
            }
            string RecieverAccountNo = input;


            bool isRecieverAccountValid = false;

            string VerifyRecieverQuery = "SELECT * FROM ATM_Users WHERE AccountNo = @recieverAccountNo";

            SqlConnection sqlConn = await _dbContext.OpenConnection();

            SqlTransaction sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                await using (SqlCommand verifyCommand = new SqlCommand(VerifyRecieverQuery, sqlConn, sqlTransaction))
                {
                    try
                    {
                        verifyCommand.Parameters.AddWithValue("@recieverAccountNo", RecieverAccountNo);

                        int result = (int)await verifyCommand.ExecuteScalarAsync();

                        if (result > 0)
                        {
                            isRecieverAccountValid = true;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Transfer failed. Incorrect Reciever's Account Number.\nPlease crosscheck the account number and try again!");
                    }
                }

                if (isRecieverAccountValid)
                {

                    Console.WriteLine("Enter amount you want to Transfer: ");
                    string input2 = Console.ReadLine();
                    while (string.IsNullOrEmpty(input2) || input2 == "0" || !decimal.TryParse(input2, out _))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid amount: ");
                        input2 = Console.ReadLine();
                    }
                    decimal TransferAmount = Convert.ToDecimal(input2);
                    

                    string BalanceQuery = "SELECT Balance FROM ATM_Users WHERE CardNumber = @cardNumber AND PIN = @pin";

                    await using SqlCommand command = new SqlCommand(BalanceQuery, sqlConn, sqlTransaction);
                    try
                    {
                        command.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                        command.Parameters.AddWithValue("@pin", User.Pin);

                        decimal Balance = (decimal)await command.ExecuteScalarAsync();

                        if (Balance < TransferAmount)
                        {
                            Console.WriteLine("You can't continue this tansaction because of Insufficient balance. Your current balance is: " + Balance);
                            await Transfer();
                        }
                        else
                        {

                            Balance -= TransferAmount;

                            string TransactionType = "Transfer";
                            DateTime TransactionTime = DateTime.Now;

                          
                            string SenderQuery = "UPDATE ATM_Users SET Balance = @balance WHERE CardNumber = @cardNumber AND PIN = @pin";
                            
                            await using SqlCommand updateCommand = new SqlCommand(SenderQuery, sqlConn, sqlTransaction);
                            
                            updateCommand.Parameters.AddWithValue("@balance", Balance);
                            updateCommand.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                            updateCommand.Parameters.AddWithValue("@pin", User.Pin);
                            await updateCommand.ExecuteNonQueryAsync();


                            string RecieverQuery = "SELECT Balance FROM ATM_Users WHERE AccountNo = @recieverAccountNo";
                            await using (SqlCommand RecieverCommand = new SqlCommand(RecieverQuery, sqlConn, sqlTransaction))
                            {
                                RecieverCommand.Parameters.AddWithValue("@recieverAccountNo", RecieverAccountNo);
                                decimal recieverBalance = (decimal)RecieverCommand.ExecuteScalar();
                                recieverBalance += TransferAmount;

                                string recieverUpdateQuery = "UPDATE ATM_Users SET Balance = @recieverBalance WHERE AccountNo = @recieverAccountNo";
                                await using (SqlCommand recieverUpdateCommand = new SqlCommand(recieverUpdateQuery, sqlConn, sqlTransaction))
                                {
                                    recieverUpdateCommand.Parameters.AddWithValue("@recieverBalance", recieverBalance);
                                    recieverUpdateCommand.Parameters.AddWithValue("@recieverAccountNo", RecieverAccountNo);

                                    await recieverUpdateCommand.ExecuteNonQueryAsync();
                                }

                                string InsertHistoryQuery = @"INSERT INTO Transaction_History (UserCardNumber, TransactionType, Amount, Date)
                                                   VALUES (@UserCardNumber, @TransactionType, @Amount, @Date)";

                                await using (SqlCommand historyCommand = new SqlCommand(InsertHistoryQuery, sqlConn, sqlTransaction))
                                {
                                    historyCommand.Parameters.AddWithValue("@UserCardNumber", User.CardNumber);
                                    historyCommand.Parameters.AddWithValue("@TransactionType", TransactionType);
                                    historyCommand.Parameters.AddWithValue("@Amount", TransferAmount);
                                    historyCommand.Parameters.AddWithValue("@Date", TransactionTime);

                                    await historyCommand.ExecuteNonQueryAsync();


                                }
                                
                                Console.WriteLine($"Transfer successful. Your new balance is: #{Balance}\n\nPress Enter to go to the Main Menu");

                                Console.ReadKey();
                                Console.Clear();
                                await AtmMenu.GetMenu();

                            }

                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: {0}", e);
                    }
                    
                }
                else
                {
                    Console.WriteLine();
                    await Transfer();
                }
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
            }

        }

        public async Task CheckBalance()
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string BalanceQuery = "SELECT Balance FROM ATM_Users WHERE CardNumber = @cardNumber AND PIN = @pin";

            using SqlCommand command = new SqlCommand(BalanceQuery, sqlConn);
            try
            {
                command.Parameters.AddWithValue("@cardNumber", User.CardNumber);
                command.Parameters.AddWithValue("@pin", User.Pin);

                decimal Balance = (decimal)await command.ExecuteScalarAsync();

                Console.WriteLine($"Your Account Balance is: #{Balance}");

                Console.ReadKey();
                Console.Clear();
                await AtmMenu.GetMenu();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
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
