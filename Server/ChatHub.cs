using Microsoft.AspNetCore.SignalR;

public class ChatHub: Hub
{
    public async Task Send(string user, string message)
    {
        if (string.IsNullOrWhiteSpace(user) ||
            string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await Clients.All.SendAsync(
            "Receive",
            $"{user}: {message}"
        );
    }
}