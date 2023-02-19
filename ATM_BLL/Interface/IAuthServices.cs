using System;
using System.Threading.Tasks;

namespace ATM_BLL.Interface
{
    public interface IAuthServices : IDisposable
    {
        Task UserLogin();

    }
}


