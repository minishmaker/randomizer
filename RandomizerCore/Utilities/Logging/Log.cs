namespace RandomizerCore.Utilities.Logging;

public class Log
{
    public Log()
    {
        Logs = new List<string>();
        ExceptionLogs = new List<ExceptionDetails>();
        HasWarningOrError = false;
    }

    public List<string> Logs { get; }

    public List<ExceptionDetails> ExceptionLogs { get; }

    public bool HasWarningOrError { get; private set; }

    public void PushLog(string log, LogType logType)
    {
        Logs.Add($"[{logType}] {log}");
        if (logType is LogType.Error or LogType.Warning) HasWarningOrError = true;
    }

    public void PushLog(Exception e)
    {
        ExceptionLogs.Add(new ExceptionDetails
        {
            StackTrace = e.StackTrace,
            Message = e.Message
        });
        HasWarningOrError = true;
    }
}
