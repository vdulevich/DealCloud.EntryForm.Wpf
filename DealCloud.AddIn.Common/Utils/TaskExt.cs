using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace DealCloud.AddIn.Common.Utils
{
    public static class TaskExt
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static ContextAwaiter WithContext(this Task task, Control control, bool alwaysAsync = false)
        {
            return new ContextAwaiter(task, control, alwaysAsync);
        }

        public static ContextAwaiter<T> WithContext<T>(this Task<T> task, Control control, bool alwaysAsync = false)
        {
            return new ContextAwaiter<T>(task, control, alwaysAsync);
        }

        public static Task<T> StartStaTask<T>(Func<T> func,ThreadPriority priority = ThreadPriority.Normal)
        {
            return StartStaTask((param) => func(), null, priority);
        }

        public static Task<T> StartStaTask<T>(Func<object, T> func, object state, ThreadPriority priority = ThreadPriority.Normal)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread((param) =>
            {
                try
                {
                    tcs.SetResult(func(param));
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Priority = priority;
            thread.Start(state);
            return tcs.Task;
        }

        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout, CancellationToken token)
        {
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, token));
            if (completedTask == task)
            {
                return await task;
            }
            if(token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            else
            {
                throw new TimeoutException("The operation has timed out.");
            }
        }

        public class ContextAwaiter : INotifyCompletion
        {
            readonly Control _control;
            readonly TaskAwaiter _awaiter;
            readonly bool _alwaysAsync;

            public ContextAwaiter(Task task, Control control, bool alwaysAsync)
            {
                _awaiter = task.GetAwaiter();
                _control = control;
                _alwaysAsync = alwaysAsync;
            }

            public ContextAwaiter GetAwaiter() { return this; }

            public bool IsCompleted => !_alwaysAsync && _awaiter.IsCompleted;

            public void OnCompleted(Action continuation)
            {
                if (_alwaysAsync || _control.InvokeRequired)
                {
                    Action<Action> callback = (c) => _awaiter.OnCompleted(c);
                    _control.BeginInvoke(callback, continuation);
                }
                else
                    _awaiter.OnCompleted(continuation);
            }

            public void GetResult()
            {
                _awaiter.GetResult();
            }
        }

        // ContextAwaiter<T>
        public class ContextAwaiter<T> : INotifyCompletion
        {
            readonly Control _control;
            readonly TaskAwaiter<T> _awaiter;
            readonly bool _alwaysAsync;

            public ContextAwaiter(Task<T> task, Control control, bool alwaysAsync)
            {
                _awaiter = task.GetAwaiter();
                _control = control;
                _alwaysAsync = alwaysAsync;
            }

            public ContextAwaiter<T> GetAwaiter() { return this; }

            public bool IsCompleted => !_alwaysAsync && _awaiter.IsCompleted;

            public void OnCompleted(Action continuation)
            {
                try
                {
                    if (_alwaysAsync || _control.InvokeRequired)
                    {
                        if (_control.IsHandleCreated)
                        {
                            Action<Action> callback = (c) => _awaiter.OnCompleted(c);
                            _control.BeginInvoke(callback, continuation);
                        }
                    }
                    else
                        _awaiter.OnCompleted(continuation);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            public T GetResult()
            {
                return _awaiter.GetResult();
            }
        }
    }
}
