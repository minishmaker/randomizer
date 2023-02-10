using Newtonsoft.Json;
using RandomizerCore.Controllers;

namespace RandomizerCore.Utilities.Logging;

internal class Logger
{
    private Log? _transaction;
    private static Logger? _instance;

    public static Logger Instance => _instance ??= new Logger();

    public Log Transaction => _transaction ??= new Log();
    public List<Log> Transactions { get; private set; }
    public bool UseVerboseLogger { get; set; } = false;
    public string OutputFilePath { get; set; }

    private Logger()
    {
        Transactions = new List<Log>();
        OutputFilePath = $"{Directory.GetCurrentDirectory()}/log.json";
    }
    public void Flush()
    {
        Transactions = new List<Log>();
        LogInfo($"Minish Cap Randomizer Core Version {ShufflerController.VersionIdentifier} {ShufflerController.RevisionIdentifier} initialized!");
        SaveLogTransaction(true);
    }
    public void LogInfo(string message)
    {
        Transaction.PushLog(message, LogType.Info);
    }

    public void LogWarning(string message)
    {
        Transaction.PushLog(message, LogType.Warning);
    }

    public void LogError(string message)
    {
        Transaction.PushLog(message, LogType.Error);
    }

    public void LogException(Exception e)
    {
        Transaction.PushLog(e);
    }

    public void SaveLogTransaction(bool forceSave = false)
    {
        if (Transaction.HasWarningOrError || UseVerboseLogger || forceSave)
            Transactions.Add(Transaction);

        _transaction = null;
    }

    public void BeginLogTransaction(bool forceSave = false)
    {
        SaveLogTransaction(forceSave);
    }

    public bool PublishLogs()
    {
        if (string.IsNullOrEmpty(OutputFilePath)) OutputFilePath = $"{Directory.GetCurrentDirectory()}/log.json";

        try
        {
            var output = JsonConvert.SerializeObject(Transactions, Formatting.Indented);
            File.WriteAllText(OutputFilePath, output);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
