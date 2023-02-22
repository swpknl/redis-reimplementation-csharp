using System.Net;
using System.Net.Sockets;
using System.Text;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

// Uncomment this block to pass the first stage
TcpListener server = new TcpListener(IPAddress.Any, 6379);
 server.Start();
var socket = server.AcceptSocket(); // wait for client
string response = "+PONG\r\n";
var bytes = Encoding.UTF8.GetBytes(response);
socket.Send(bytes);