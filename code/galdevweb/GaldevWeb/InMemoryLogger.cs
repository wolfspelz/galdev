#nullable disable
using System.Collections.Concurrent;

namespace GaldevWeb;

public class InMemoryLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentQueue<string> _logs = new ConcurrentQueue<string>();

    public ILogger CreateLogger(string categoryName)
    {
        return new InMemoryLogger(_logs);
    }

    public void Dispose() { }

    public ConcurrentQueue<string> GetLogs() => _logs;
}

public class InMemoryLogger : ILogger
{
    private readonly ConcurrentQueue<string> _logs;

    public InMemoryLogger(ConcurrentQueue<string> logs)
    {
        _logs = logs;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        _logs.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}");
    }
}
