using System.ComponentModel.DataAnnotations;

namespace DateClassLibrary.Data
{
    /// <summary>
    /// 创建用户的密码和用户重置密码
    /// </summary>
    public class UserPassword
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
