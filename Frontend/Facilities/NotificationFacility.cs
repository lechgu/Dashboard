using Microsoft.AspNetCore.SignalR.Client;

namespace Dashboard.Frontend.Facilities;

public class NotificationFacility
{
    HubConnection hubConnection;
    bool isConnected = false;

    public event Action NewEvent;

    public async Task ConnectAsync()
    {
        if (hubConnection is null)
        {
            var url = "https://localhost:5003/hubs/notifications";
            hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();
            hubConnection.On("NewEvent", () => NewEvent?.Invoke());

        }
        if (!isConnected)
        {
            await hubConnection.StartAsync();
            isConnected = true;
        }
    }
}