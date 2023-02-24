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
        List<string> command = new List<string>();
        var commands = message.Split("$");
        for(int i = 0; i < commands.Length; i++) 
        {
            command.Add(commands[i].Trim().Substring(1).Trim());
        }
        
        result.Add(this.GetCommandResponse(command[1], command));
        return String.Join("\r\n", result);
    }

    private string GetCommandResponse(string command, List<string> data)
    {
        string result = null;
        switch(command)
        {
            case "ping":
                result = "PONG";
                break;
            case "echo":
                result = data[2];
                break;
            case "set":
                this.cache.Set(data[2], data[3], this.GetTtl(data));
                result = "OK";
                break;
            case "get":
                return this.cache.Get(data[2]);
        }

        return $"+{result}\r\n";
    }

    private int? GetTtl(List<string> data)
    {
        if(data.Contains("px"))
        {
            int index = data.IndexOf("px");
            return int.Parse(data[index + 1]);
        }
        else
        {
            return null;
        }
    }
}
