using System.Net;
using System.Net.Sockets;
using System.Text;

class Program {
    public static void Main(String[] args) {
        IPHostEntry hostEntry = Dns.GetHostEntry("localhost");
        IPAddress address = hostEntry.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(address, 6379);
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(endPoint);
        socket.Listen(100);
        var handler = socket.Accept();
        while(true) {
            try {
                byte[] requestBytes = new byte[1024];
                var messageRequest = handler.Receive(requestBytes);
                var message = Encoding.UTF8.GetString(requestBytes, 0, messageRequest);
                var response = Encoding.UTF8.GetBytes(GetResponse(message));
                handler.Send(response);
            } catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static String GetResponse(string message) {
        if(message == "*1\r\n$4\r\nping\r\n") {
            return "+PONG\r\n";
        } else if(message == "ping\r\nping") {
            Console.WriteLine("in multiple ping");
            return "+PONG\r\n+PONG\r\n";
        } else {
            return string.Empty;
        }
    }

    private static string GetRequest(byte[] bytes) {
        return Encoding.UTF8.GetString(bytes);
    }
}
