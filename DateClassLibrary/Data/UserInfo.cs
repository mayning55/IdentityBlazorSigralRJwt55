namespace DateClassLibrary.Data
{
    /// <summary>
    /// 与CreateUser一样，这是浏览用户时用到的属性。
    /// </summary>
    public class UserInfo
    {
        public string Id {  get; set; }
        public string UserName { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreateDatetime { get; set; }
        public Position Position { get; set; }
        public string Email { get; set; }
    }
}
