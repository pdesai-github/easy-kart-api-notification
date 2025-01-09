using SharedModel = EasyKart.Shared.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace EasyKart.Notification
{
    public class NotificationConsumer : IConsumer<SharedModel.Notification>
    {
        IHubContext<NotificationHub> hubContext;

        public NotificationConsumer(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<SharedModel.Notification> context)
        {
            await hubContext.Clients.All.SendAsync("ReceiveMessage", context.Message);
        }
    }
}
