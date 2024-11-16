using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Hubs
{
    public class NoteHub:Hub
    {
        public async Task SendMessage(string notemsg)
        {
            await Clients.All.SendAsync("NoteMessage", notemsg);
        }
    }
}
