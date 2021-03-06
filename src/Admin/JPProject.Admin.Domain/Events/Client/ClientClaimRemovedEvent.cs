using JPProject.Domain.Core.Events;

namespace JPProject.Admin.Domain.Events.Client
{
    public class ClientClaimRemovedEvent : Event
    {
        public string Type { get; }
        public string Value { get; }

        public ClientClaimRemovedEvent(string type, string value, string clientId)
        {
            Type = type;
            Value = value;
            AggregateId = clientId;
        }
    }
}