using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace Broker
{
    class Program
    {
        private const string FRONT_ENDPOINT = "tcp://*:5559";
        private const string BACK_ENDPINT = "tcp://*:5560";
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            using (Socket frontend = context.Socket(SocketType.ROUTER), backend = context.Socket(SocketType.DEALER))
            {
                frontend.Bind(FRONT_ENDPOINT);
                backend.Bind(BACK_ENDPINT);

                Console.WriteLine("Broker starting ...");

                Socket.Device.Queue(frontend, backend);
            }
        }
    }
}
