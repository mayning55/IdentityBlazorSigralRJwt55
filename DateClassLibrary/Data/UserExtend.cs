using Microsoft.AspNetCore.Identity;

namespace DateClassLibrary.Data
{
    public class UserExtend : IdentityUser
    {
        public bool IsDisabled { get; set; } //扩展属性，下同。
        public DateTime CreateDatetime { get; set; }
        public long JWTVer{ get; set; }
        public Position Position { get; set; }//todo数据权限，Manager管理全部，General仅自己。！！！！又或者使用identity的Claims
    }
    public enum Position
    {
        Manager, General
    }
}
