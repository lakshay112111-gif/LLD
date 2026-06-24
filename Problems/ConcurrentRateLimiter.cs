using System;
using System.Collections.Concurrent;
using System.Threading;

// Rate Limit Config

public class RateLimitConfig
{
public int Capacity { get; }

public int LeakRatePerSecond { get; }

public RateLimitConfig(
    int capacity,
    int leakRatePerSecond)
{
    Capacity = capacity;
    LeakRatePerSecond = leakRatePerSecond;
}

}

// User-specific Context

public class RateLimitContext
{
public string UserId { get; }

public int CurrentBucketSize { get; set; }

public DateTime LastLeakTime { get; set; }

public RateLimitContext(string userId)
{
    UserId = userId;
    CurrentBucketSize = 0;
    LastLeakTime = DateTime.UtcNow;
}
}

// Strategy Interface

public interface IRateLimiterStrategy
{
bool AllowRequest(
RateLimitContext context,
RateLimitConfig config);
}

// Leaky Bucket Strategy

public class LeakyBucketStrategy
: IRateLimiterStrategy
{
public bool AllowRequest(
RateLimitContext context,
RateLimitConfig config)
{
lock (context)
{
DateTime now = DateTime.UtcNow;

        double elapsedSeconds =
            (now - context.LastLeakTime)
            .TotalSeconds;

        int leakedRequests =
            (int)(elapsedSeconds *
                  config.LeakRatePerSecond);

        context.CurrentBucketSize =
            Math.Max(
                0,
                context.CurrentBucketSize -
                leakedRequests);

        context.LastLeakTime = now;

        if (context.CurrentBucketSize >= config.Capacity)
        {
            return false;
        }

        context.CurrentBucketSize++;

        return true;
    }
}

}

// Rate Limiter

public class RateLimiter
{
private readonly IRateLimiterStrategy
_strategy;

private readonly RateLimitConfig
    _config;

private readonly ConcurrentDictionary<
    string,
    RateLimitContext> _userContexts;

public RateLimiter(
    IRateLimiterStrategy strategy,
    RateLimitConfig config)
{
    _strategy = strategy;
    _config = config;

    _userContexts =
        new ConcurrentDictionary<
            string,
            RateLimitContext>();
}

public bool AllowRequest(
    string userId)
{
    RateLimitContext context =
        _userContexts.GetOrAdd(
            userId,
            id => new RateLimitContext(id));

    return _strategy.AllowRequest(
        context,
        _config);
}

}

// Client

public class Program
{
public static void Main()
{
RateLimitConfig config =
new RateLimitConfig(
capacity: 5,
leakRatePerSecond: 1);
    IRateLimiterStrategy strategy =
        new LeakyBucketStrategy();

    RateLimiter rateLimiter =
        new RateLimiter(
            strategy,
            config);

    string userId = "user1";

    Console.WriteLine(
        "Sending 7 requests...\n");

    for (int i = 1; i <= 7; i++)
    {
        bool allowed =
            rateLimiter.AllowRequest(
                userId);

        Console.WriteLine(
            $"Request {i}: {(allowed ? "ALLOWED" : "REJECTED")}");
    }

    Console.WriteLine();
    Console.WriteLine(
        "Waiting 3 seconds...\n");

    Thread.Sleep(3000);

    Console.WriteLine(
        "Sending 3 more requests...\n");

    for (int i = 8; i <= 10; i++)
    {
        bool allowed =
            rateLimiter.AllowRequest(
                userId);

        Console.WriteLine(
            $"Request {i}: {(allowed ? "ALLOWED" : "REJECTED")}");
    }
}
}
