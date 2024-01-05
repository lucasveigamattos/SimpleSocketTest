using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace SocketServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {
        private IChat Chat { get; }

        public SocketController(IChat chat)
        {
            this.Chat = chat;
        }

        [Route("connect-socket")]
        public async Task ConnectSocket()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                if (this.Chat.Messages.Count > 0) await this.SendMessages(webSocket);

                int messagesLength = this.Chat.Messages.Count;

                while (true)
                {
                    if (messagesLength != this.Chat.Messages.Count)
                    {
                        messagesLength = this.Chat.Messages.Count;

                        byte[] bytes = Encoding.UTF8.GetBytes(this.Chat.Messages[this.Chat.Messages.Count - 1]);
                        ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
                        await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                    }
                }
            } else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        public async Task SendMessages(WebSocket webSocket)
        {
            foreach (string message in this.Chat.Messages)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
                await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }

        [HttpPost("send-message")]
        public IActionResult SendMessage([FromBody] MessageRequest messageRequest)
        {
            this.Chat.AddMessage(messageRequest.Message);
            return Ok();
        }

        public class MessageRequest
        {
            public string Message { get; set; }
        }
    }
}