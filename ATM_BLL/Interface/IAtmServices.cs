using System;

namespace ATM_BLL.Interface
{
    public interface IAtmServices : IDisposable
    {
        void Withdraw();
        void Transfer();
        void CheckBalance();
        void Deposit();

    }
}