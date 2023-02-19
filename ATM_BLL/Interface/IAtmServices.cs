using System;
using System.Threading.Tasks;

namespace ATM_BLL.Interface
{
    public interface IAtmServices : IDisposable
    {
        Task Withdraw();
        Task Transfer();
        Task CheckBalance();
        Task Deposit();

    }
}