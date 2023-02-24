using System.Text;

public class RedisRequestParser
{
    private readonly RedisCache cache;

    public RedisRequestParser()
    {
        this.cache = new RedisCache();
    }

    public string Parse(string message)
    {
        List<string> result = new List<string>();
        var data = message.TrimEnd().Split("\r\n");
        // data.ToList().ForEach(x => Console.Write(x));
        for (int i = 1; i < data.Length; i += 2)
        {
            var inp = data[i].Replace("$", string.Empty).Trim();
            var length = int.Parse(inp);

            if (length < 0)
            {
                result.Add("nil");
            }
            else
            {
                var command = data[i + 1];
                result.Add(this.GetCommandResponse(command, data, ref i));
            }
        }

        return String.Join("\r\n", result);
    }

    private string GetCommandResponse(string command, string[] data, ref int index)
    {
        string result = null;
        switch(command)
        {
            case "ping":
                result = "PONG";
                break;
            case "echo":
                result = data[index + 3];
                break;
            case "set":
                this.cache.Set(data[index + 3], data[index + 5]);
                index += 5;
                result = "OK";
                break;
            case "get":
                result = this.cache.Get(data[index + 3]);
                index += 3;
                break;
            default: 
                result = "PONG";
                break;
        }

        return $"+{result}\r\n";
    }
}
