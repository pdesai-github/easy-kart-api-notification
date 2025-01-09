using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models = EasyKart.Shared.Models;

namespace EasyKart.Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private IHubContext<NotificationHub> hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Notification message)
        {
            // Send notification to user
            await hubContext.Clients.All.SendAsync("ReceiveMessage", message.UserId, message.Message);
            return Ok(new { Status = "Message sent" });
        }
    }
}
