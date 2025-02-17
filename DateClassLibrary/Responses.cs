namespace DateClassLibrary
{
    /// <summary>
    /// API接口返回的信息
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    public record Responses(bool Flag, string Message = null!);

}
