using System.Net;
using System.Reflection;
using Newtonsoft.Json;

namespace MinishCapRandomizerCLI;

public static class Helpers
{
    public static void CheckForUpdates()
    {
        var githubData = DownloadUrlAsString();
        var releases = JsonConvert.DeserializeObject<List<Release>>(githubData);
        var tag = Assembly.GetExecutingAssembly().GetCustomAttributesData().First(x => x.AttributeType.Name == "AssemblyInformationalVersionAttribute").ConstructorArguments.First().ToString();
        tag = tag[(tag.IndexOf('+') + 1)..^1];
        if (releases!.First().Tag_Name == tag) return;
        
        Console.WriteLine(@$"
**************************************************************************************************
A new version of the Legend of Zelda Minish Cap Randomizer is available! You can download it here:
{releases!.First().Html_Url}
**************************************************************************************************
");
    }
        
    private static string DownloadUrlAsString()
    {
        var request = (HttpWebRequest)WebRequest.Create("https://api.github.com/repositories/177660043/releases");
        request.UserAgent = "MinishCapRandomizerUI";
        request.KeepAlive = false;
        using var response = (HttpWebResponse)request.GetResponse();
        using var responseStream = new StreamReader(response.GetResponseStream());
        return responseStream.ReadToEnd();
    }
}
