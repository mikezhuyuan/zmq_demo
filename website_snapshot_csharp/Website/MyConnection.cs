using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SignalR;
using ZMQ;

namespace Website
{
    public class MyConnection : PersistentConnection
    {        
        private Context context = new Context(1);
        private const string REQUEST_ENDPOINT = "tcp://localhost:5559";
        private const string RESOLUTION = "1024 768";
        protected override Task OnReceivedAsync(IRequest request, string connectionId, string url)
        {
            url = url.Trim();
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var image = Request(url);
                return Connection.Send(connectionId, image);
            }

            return Connection.Send(connectionId, "notfound.png");
        }

        private string Request(string url)
        {
            using (var socket = context.Socket(SocketType.REQ))
            {
                socket.Connect(REQUEST_ENDPOINT);

                socket.Send(url + " " + RESOLUTION, Encoding.Unicode);
                return socket.Recv(Encoding.Unicode);
            }
        }
    }
}