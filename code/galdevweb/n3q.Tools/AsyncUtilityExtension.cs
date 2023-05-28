using System;
using System.Threading;
using System.Threading.Tasks;

namespace n3q.Tools
{
    public static class AsyncUtilityExtension
    {
        public static void PerformAsyncTaskWithoutAwait(this Task task, Action<Task> exceptionHandler)
        {
            var dummy = task?.ContinueWith(t => exceptionHandler(t), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Default);
        }
    }
}
