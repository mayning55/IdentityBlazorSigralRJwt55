namespace LoginClassLibrary.Account
{
    /// <summary>
    /// 客户端保存的Token和用于刷新的RefreshToken
    /// </summary>
    public class UserSession
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
