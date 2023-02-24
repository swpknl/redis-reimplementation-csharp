using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class AsyncServer
{
    TcpListener listener;
    ConcurrentDictionary<Socket, NetworkStream> map =
        new ConcurrentDictionary<Socket, NetworkStream>();

    public void Start()
    {
        this.listener = this.CreateTcpListener();
        this.listener.Start();
        this.StartEventLoop();
    }

    private void StartEventLoop()
    {
        while (true)
        {
            this.listener.BeginAcceptSocket(AcceptSocket, null);
        }
    }

    private async void AcceptSocket(IAsyncResult result)
    {
        var socket = this.listener.EndAcceptSocket(result);
        var stream = new NetworkStream(socket);
        while (stream.CanRead)
        {
            var request = await this.GetRequest(stream);
            var response = this.Process(request);
            await this.SendResponse(response, stream);
        }
    }

    private TcpListener CreateTcpListener()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Constants.Port);
        TcpListener listener = new TcpListener(endPoint);
        return listener;
    }

    private async Task SendResponse(string processResponse, NetworkStream stream)
    {
        var response = new ArraySegment<byte>(Encoding.UTF8.GetBytes(processResponse));
        await stream.WriteAsync(response.Array);
    }

    private string Process(string request)
    {
        var response = GetResponse(request);
        return response;
    }

    private String GetResponse(string message)
    {
        // TODO: Write Parser for Redis request
        return "+PONG\r\n";
    }

    private async Task<string> GetRequest(NetworkStream stream)
    {
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
        var messageRequest = await stream.ReadAsync(buffer.Array, 0, buffer.Array.Length);
        var request = Encoding.UTF8.GetString(buffer.Array, 0, messageRequest);
        return request;
    }
}
