namespace RandomizerCore.Controllers.Models
{
	public class ShufflerControllerResult
	{
		public bool WasSuccessful { get; set; }

		public Exception? Error { get; set; }

		public string? ErrorMessage { get; set; }
	}
}
