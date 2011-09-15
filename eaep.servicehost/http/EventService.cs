using eaep.servicehost.store;

namespace eaep.servicehost.http
{
    public class EventService : HttpService
    {
        IEAEPMonitorStore monitorStore;

        public EventService(IEAEPMonitorStore monitorStore)
        {
            this.monitorStore = monitorStore;
        }

        public override void ProcessRequest(IServiceRequest request, IServiceResponse response, IResourceRepository resourceRepository)
        {
            EAEPMessages messages = new EAEPMessages(request.Body);
            monitorStore.PushMessages(messages);
            response.StatusCode = Constants.HTTP_200_OK;
            response.StatusDescription = "OK";
        }
    }
}
