using ATM_BLL.Interface;
using ATM_DAL.Database;
using System;

namespace ATM_BLL.Implementation
{
    public delegate string Mydelegate(string message);

    public class starter
    {
        public static void Run()
        {
            Mydelegate mydelegate = new Mydelegate(PrintMessage);
            mydelegate += PrintMessage;

            string output = mydelegate.Invoke("...............Welcome to WAHALA ATM!................ \n.....Kindly Login with your details......\n");
            Console.WriteLine(output);

            IAuthServices auth = new AuthServices(new AtmDbContext());
            auth.UserLogin();

        }


        public static string PrintMessage(string message)
        {
            return message;
        }

    }



}

