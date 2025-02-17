namespace DateClassLibrary.Data
{
    /// <summary>
    /// 用户刷新Token
    /// </summary>
    public class RefreshTokenInfo
    {
        public long Id { get; set; }
        public string? Token { get; set; }
        public string? UserName { get; set; }
    }
}
