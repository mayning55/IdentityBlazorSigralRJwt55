using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    /// <summary>
    /// Person变更后发送信息给在线用户，重新加载页面。
    /// </summary>
    public class NoteHub : Hub
    {
        public async Task SendMessage(string notemsg)
        {
            await Clients.All.SendAsync("NoteMessage", notemsg);
        }
    }
}
