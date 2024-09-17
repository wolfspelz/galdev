#nullable disable
using System.Collections.Concurrent;

namespace GaldevWeb;

public class InMemoryLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentQueue<string> _logs = new ConcurrentQueue<string>();
    private readonly int _maxLines = 1000;

    public InMemoryLoggerProvider(int maxLines)
    {
        _maxLines = maxLines;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new InMemoryLogger(_logs, _maxLines);
    }

    public void Dispose() { }

    public ConcurrentQueue<string> GetLogs() => _logs;
}

public class InMemoryLogger : ILogger
{
    private readonly ConcurrentQueue<string> _logs;
    private readonly int _maxLines = 1000;

    public InMemoryLogger(ConcurrentQueue<string> logs, int maxLines)
    {
        _logs = logs;
        _maxLines = maxLines;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        _logs.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}");
        TruncateLogs();
    }

    private void TruncateLogs()
    {
        while (_logs.Count >= _maxLines) {
            _logs.TryDequeue(out _);
        }
    }
}
