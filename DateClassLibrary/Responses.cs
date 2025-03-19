namespace DateClassLibrary
{
    /// <summary>
    /// API接口返回的信息
    /// 自定义响应返回指定对象信息，可以添加实际的业务对象数据。但这里仅用作接口信息响应。
    /// 参阅：https://www.51cto.com/article/785224.html    
    /// </summary>
    /// <param name="Flag"></param>
    /// <param name="Message"></param>
    public record Responses(bool Flag, string Message = null!);

}
/*
例子：
    /// public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}

// 如果不需要泛型类型的数据，也可以创建一个非泛型的返回类
public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    // 如果有需要，也可以添加其他非业务数据字段
}

[HttpGet]
[Route("api/users/{id}")]
public async Task<IHttpActionResult> GetUser(int id)
{
    try
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new ApiResponse<User> { StatusCode = 404, Message = "User not found" });
        }

        return Ok(new ApiResponse<User> { StatusCode = 200, Message = "User found", Data = user });
    }
    catch (Exception ex)
    {
        // 处理异常并返回错误信息
        return InternalServerError(new ApiResponse { StatusCode = 500, Message = "Internal server error: " + ex.Message });
    }
}*/