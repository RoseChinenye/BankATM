using ATM_BLL.Implementation;
using ATM_DAL.Database;

namespace ATM_UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Creating Database
            //CreateAtmDB.CreateDatabase();


            //Creating Default Atm users
            //CreateUsers atmUsers = new CreateUsers(new AtmDbContext());
            //atmUsers.CreateAtmUsers();

            //Creating Transaction History Table
            //CreateTransactionHistory history = new CreateTransactionHistory(new AtmDbContext());
            //history.TransactionHistory();

            starter.Run();
        }
    }
}