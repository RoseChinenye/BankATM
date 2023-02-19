using ATM_BLL.Interface;
using ATM_DAL.Database;
using System;
using System.Threading.Tasks;

namespace ATM_BLL.Implementation
{
    public delegate string Mydelegate(string message);

    public class starter
    {
        public static async Task Run()
        {
            Mydelegate mydelegate = new Mydelegate(PrintMessage);
            mydelegate += PrintMessage;

            //Create Database
            await CreateAtmDB.CreateDatabase();

            //Create Default Atm users
            CreateUsers atmUsers = new CreateUsers(new AtmDbContext());
            await atmUsers.CreateAtmUsers();

            //Creating Transaction History Table
            CreateTransactionHistory history = new CreateTransactionHistory(new AtmDbContext());
            await history.TransactionHistory();


            string output = mydelegate.Invoke("...............Welcome to WAHALA ATM!................ \n.....Kindly Login with your details......\n");
            Console.WriteLine(output);

            //Login
            IAuthServices auth = new AuthServices(new AtmDbContext());
            await auth.UserLogin();


        }


        public static string PrintMessage(string message)
        {
            return message;
        }

    }



}

