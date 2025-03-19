using System.ComponentModel.DataAnnotations;

namespace DateClassLibrary.Data
{
    public class CreateUser : UserPassword
    {
        /// <summary>
        /// 创建用户时用到的属性，避免过度发布，也可以用TryUpdateModelAsync
        /// https://learn.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.tryupdatemodelasync?view=aspnetcore-8.0#microsoft-aspnetcore-mvc-controllerbase-tryupdatemodelasync(system-object-system-type-system-string)
        /// </summary>
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public Position Position { get; set; }

    }
}
