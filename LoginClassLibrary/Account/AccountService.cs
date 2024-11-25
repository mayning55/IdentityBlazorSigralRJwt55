using LoginClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LoginClassLibrary.Account
{
    /// <summary>
    /// 通过WebAPI提交登录
    /// </summary>
    public class AccountService : IAccount
    {
        private readonly HttpClient httpClient;

        public AccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<LoginResponse> LogInAccountAsync(LoginRequest model)
        {
            var response = await httpClient.PostAsJsonAsync("account/login", model);
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result;
        }
    }
 }
