using ATM_BLL.Interface;
using ATM_DAL.Database;
using System;
using System.Threading.Tasks;

namespace ATM_BLL.Implementation
{
    public class AtmMenu
    {
        public static async Task GetMenu()
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
                        await services.Deposit();
                        break;

                    case 2:
                        await services.CheckBalance();
                        break;

                    case 3:
                        await services.Withdraw();
                        break;

                    case 4:
                        await services.Transfer();
                        break;

                    case 5:
                        await AuthServices.UserLogout();
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Your input appears to be invalid, please try a numeric value");
                await GetMenu();
            }
        }
    }

}
