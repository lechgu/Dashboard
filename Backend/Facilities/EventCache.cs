using Dashboard.Backend.Hubs;
using Dashboard.Shared;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.SignalR;

namespace Dashboard.Backend.Facilities;

public class EventCache
{
    static readonly List<Event> cache = new();
    static readonly object mutex = new();
    private readonly IHubContext<NotificationHub> notificationHub;

    public EventCache(IHubContext<NotificationHub> notificationHub)
    {
        this.notificationHub = notificationHub;
    }

    public async Task AddEvent(RawEvent rawEvent)
    {
        var ev = new Event
        {
            Description = rawEvent.Description,
            Category = rawEvent.Category,
            Severity = rawEvent.Severity,
            PublishedAt = Timestamp.FromDateTime(DateTime.UtcNow)
        };
        lock (mutex)
        {
            cache.Add(ev);
        }
        await notificationHub.Clients.All.SendAsync("NewEvent");
    }

    public IEnumerable<Event> GetEvents(int skip, int take)
    {
        var result = new List<Event>();
        lock (mutex)
        {
            result.AddRange(cache.Skip(skip).Take(take));
        }
        return result;
    }
}