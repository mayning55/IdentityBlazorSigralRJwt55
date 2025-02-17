using System.ComponentModel.DataAnnotations;


namespace LoginClassLibrary
{
    /// <summary>
    /// 用户登录提交的请求字段。
    /// </summary>
    public class LoginRequest
    {
        [Required]
        public string? UserName { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;
    }
}
