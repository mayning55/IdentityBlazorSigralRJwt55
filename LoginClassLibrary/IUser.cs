using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginClassLibrary
{
    public interface IUser
    {
        Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest);
    }
}
