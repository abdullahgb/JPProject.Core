using JPProject.Domain.Core.Events;

namespace JPProject.Admin.Domain.Events.ApiResource
{
    public class ApiResourceRemovedEvent : Event
    {
        public ApiResourceRemovedEvent(string name)
        {
            AggregateId = name;
        }
    }
}