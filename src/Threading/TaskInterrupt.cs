using System;
using System.Threading;
using System.Threading.Tasks;

namespace CilDotNet.Threading
{
    public sealed class TaskInterrupt : IDisposable
    {
        private readonly CancellationTokenSource cts;
        private bool disposed;

        public CancellationToken Token => cts.Token;
        public bool Running { get; private set; }
        public bool IsCancellationRequested => cts.IsCancellationRequested;

        public event Action? OnCancelled;

        public TaskInterrupt()
        {
            cts = new CancellationTokenSource();
            Running = true;
            Console.CancelKeyPress += OnCancelKeyPress!;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit!;
        }

        public void Cancel()
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
                Running = false;
                OnCancelled?.Invoke();
            }
        }

        public static bool WaitAll(params Task[] tasks)
        {
            try
            {
                return Task.WaitAll(tasks, TimeSpan.FromSeconds(5));
            }
            catch
            {
                return false;
            }
        }

        public Task Run(Action<CancellationToken> action)
        {
            return Task.Run(() => action(Token), Token);
        }

        public Task<T> Run<T>(Func<CancellationToken, T> func)
        {
            return Task.Run(() => func(Token), Token);
        }

        private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Cancel();
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            Cancel();
        }

        public void Dispose()
        {
            if (disposed) return;
            Console.CancelKeyPress -= OnCancelKeyPress;
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            cts?.Cancel();
            cts?.Dispose();
            disposed = true;
            Running = false;
        }
    }
}