namespace LoginClassLibrary
{
    /// <summary>
    /// 用户登录后的响应
    /// </summary>
    /// <param name="Flag"></param>：是否登录成功；
    /// <param name="Message"></param>：说明信息；
    /// <param name="Token"></param>：登录成功生成的Token；
    /// <param name="RefreshToken"></param>：用于刷新Token验证的Hash信息。
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!, string RefreshToken = null!)
    {
    }
}
