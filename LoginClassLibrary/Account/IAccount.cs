﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginClassLibrary.Account
{
    public interface IAccount
    {
        Task<LoginResponse> LogInAccountAsync(LoginRequest model);
    }
}