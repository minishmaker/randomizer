namespace RandomizerCore.Controllers.Models;

public class ShufflerControllerResult
{
    public bool WasSuccessful { get; set; }

    public Exception? Error { get; set; }

    public string? ErrorMessage { get; set; }

    public static implicit operator bool(ShufflerControllerResult result)
    {
        return result.WasSuccessful;
    }
}
