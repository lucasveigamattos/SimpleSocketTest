using System.Net.WebSockets;

namespace SocketServer
{
    public class Chat : IChat
    {
        public List<string> Messages { get; set; }

        public Chat() 
        {
            this.Messages = new List<string>();
        }

        public void AddMessage(string message) => this.Messages.Add(message);
    }

    public interface IChat
    {
        List<string> Messages { get; set; }
        void AddMessage(string message);
    }
}