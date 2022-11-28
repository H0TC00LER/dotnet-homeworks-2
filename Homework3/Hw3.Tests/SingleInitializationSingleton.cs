using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static Lazy<SingleInitializationSingleton> instance
        = new Lazy<SingleInitializationSingleton>(() => new SingleInitializationSingleton());

    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    public const int DefaultDelay = 3_000;

    public int Delay { get; }

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    internal static void Reset()
    {
        _isInitialized = false;
    }

    public static void Initialize(int delay)
    {
        lock (Locker)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                instance = new Lazy<SingleInitializationSingleton>
                    (() => new SingleInitializationSingleton(delay));
            }
            else
                throw new InvalidOperationException();
        }
    }

    public static SingleInitializationSingleton Instance => instance.Value;
}