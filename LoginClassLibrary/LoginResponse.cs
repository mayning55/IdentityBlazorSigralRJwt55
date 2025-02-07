using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginClassLibrary
{
    /// <summary>
    /// 登录后返回的信息，
    /// </summary>
    /// <param name="Flag"></param>是否登录成功
    /// <param name="Message"></param>返回信息
    /// <param name="Token"></param>Token
    /// <param name="RefreshToken"></param>刷新Token的Hash值
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!, string RefreshToken = null!)
    {
    }
}
