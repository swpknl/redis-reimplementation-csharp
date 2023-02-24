using System.Text;

public class RedisRequestParser
{
    private readonly string message;
    private readonly RedisCache cache;

    public RedisRequestParser(string message)
    {
        this.message = message;
        this.cache = new RedisCache();
    }

    public string Parse()
    {
        List<string> result = new List<string>();
        var data = this.message.TrimEnd().Split("\r\n");
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
                result.Add(this.GetCommandResponse(command, data, i));
            }
        }

        return String.Join("\r\n", result);
    }

    private string GetCommandResponse(string command, string[] data, int index)
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
                this.cache.Set("", "");
                break;
            case "get":
                result = this.cache.Get("");
                break;
            default: 
                result = "PONG";
                break;
        }

        return $"+{result}\r\n";
    }
}
