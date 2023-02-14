using ATM_BLL.Interface;
using ATM_DAL.Database;
using System;

namespace ATM_BLL.Implementation
{
    public class AtmMenu
    {
        public static void GetMenu()
        {
            Console.WriteLine("Select any of the options");
            Console.WriteLine("Enter 1 to Deposit\nEnter 2 to Check balance\nEnter 3 to Withdraw\nEnter 4 to Transfer\nEnter 5 to Log Out");

            var selection = Console.ReadLine();

            IAtmServices services = new AtmServices(new AtmDbContext());

            if (int.TryParse(selection, out int userInput))
            {
                switch (userInput)
                {
                    case 1:
                        services.Deposit();
                        break;

                    case 2:
                        services.CheckBalance();
                        break;

                    case 3:
                        services.Withdraw();
                        break;

                    case 4:
                        services.Transfer();
                        break;

                    case 5:
                        AuthServices.UserLogout();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Your input appears to be invalid, please try a numeric value");
                GetMenu();
            }
        }
    }

}
