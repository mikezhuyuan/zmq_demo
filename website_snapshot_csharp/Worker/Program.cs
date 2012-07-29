using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZMQ;
using Exception = System.Exception;

namespace Worker
{
    class Program
    {
        private const string ENDPOINT = "tcp://localhost:5560";
        private const string IMG_FOLDER = "../Website/imgs/";
        //Example request: http://website 1024 768
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            using (var socket = context.Socket(SocketType.REP))
            {
                socket.Connect(ENDPOINT);
                Console.WriteLine(DateTime.Now.TimeOfDay + " Listening...");
                while (true)
                {
                    string request = socket.Recv(Encoding.Unicode);
                    Console.WriteLine(DateTime.Now.TimeOfDay + " Processing: " + request);
                    var image = SaveImage(request);
                    Console.WriteLine(DateTime.Now.TimeOfDay + " Sending image name:" + image);
                    socket.Send(image, Encoding.Unicode);
                    Console.WriteLine(DateTime.Now.TimeOfDay + " Done.");
                }
            }
        }

        private static string SaveImage(string request)
        {
            try
            {
                var parts = request.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var url = parts[0];
                int width = int.Parse(parts[1]), height = int.Parse(parts[2]);
                var name = request.GetHashCode() + ".png"; // TODO: use MD5
                var path = IMG_FOLDER + name;
                if(!System.IO.File.Exists(path))
                    RenderURL(url, width, height, path);

                return name;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return null;
            }
        }

        private static void RenderURL(string url, int width, int height, string imagePath)
        {            
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "phantomjs.exe";
            p.StartInfo.Arguments = string.Format("render.coffee {0} {1} {2} {3}", url, width, height, imagePath);
            p.Start();

            p.WaitForExit();
        }
    }
}