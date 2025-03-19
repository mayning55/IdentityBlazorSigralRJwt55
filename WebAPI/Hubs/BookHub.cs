using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs;
/// <summary>
/// Book变更后发送信息给在线用户，重新加载页面。
/// </summary>
public class BookHub : Hub
{
    public async Task SendMessage(string notemsg)
    {
        await Clients.All.SendAsync("BookMessage", notemsg);
    }
}
