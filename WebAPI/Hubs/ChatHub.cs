using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    /// <summary>
    /// 组件添加到天气页面，
    /// </summary>
    public class ChatHub:Hub
    {
        public async Task SendMessage(string user,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",user, message);
        }
    }
}
