using System.Reflection;
using Newtonsoft.Json;
using RestSharp;

namespace MinishCapRandomizerCLI;

public static class Helpers
{
    public static async void CheckForUpdates()
    {
        var githubData = await DownloadUrlAsString();
        if (string.IsNullOrEmpty(githubData))
            return;
        
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
        
    private static async Task<string> DownloadUrlAsString()
    {
        var client = new RestClient();
        var request = new RestRequest("https://api.github.com/repositories/177660043/releases", Method.Get);
        try
        {
            var result = await client.ExecuteAsync(request);

            if (result.IsSuccessful && !string.IsNullOrEmpty(result.Content)) return result.Content;
            
            Console.WriteLine($"Server returned status code {result.StatusCode} with message {result.ErrorMessage}");
            return "";
        }
        catch (Exception e)
        {
            Console.WriteLine($"Web request threw exception: {e.Message}");
            return "";
        }
    }
}
