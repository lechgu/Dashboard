using Dashboard.Backend.Facilities;
using Dashboard.Shared;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static Dashboard.Shared.Collector;

namespace Dashboard.Backend.GrpcServices;

public class CollectorService : CollectorBase
{
    private readonly EventCache cache;

    public CollectorService(EventCache cache)
    {
        this.cache = cache;
    }

    public override async Task<Empty> Publish(RawEvent rawEvent, ServerCallContext context)
    {
        await cache.AddEvent(rawEvent);
        return new Empty();
    }
}