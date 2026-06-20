using System;
using System.Collections.Generic;

// Observer Pattern Example - YouTube Channel & Subscribers

public interface IChannel
{
    void Subscribe(ISubscriber subscriber);
    void Unsubscribe(ISubscriber subscriber);
    void NotifySubscribers();
}

public interface ISubscriber
{
    void Update(string videoName);
}

// Subject
public class Channel : IChannel
{
    private string _channelName;
    private string _latestVideo;

    private List<ISubscriber> _subscribers =
        new List<ISubscriber>();

    public Channel(string channelName)
    {
        _channelName = channelName;
    }

    public void Subscribe(ISubscriber subscriber)
    {
        if (!_subscribers.Contains(subscriber))
        {
            _subscribers.Add(subscriber);
        }
    }

    public void Unsubscribe(ISubscriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Update(_latestVideo);
        }
    }

    public void UploadVideo(string videoName)
    {
        _latestVideo = videoName;

        Console.WriteLine(
            $"\n{_channelName} uploaded: {_latestVideo}");

        NotifySubscribers();
    }
}

// Observer
public class Subscriber : ISubscriber
{
    private string _name;

    public Subscriber(string name)
    {
        _name = name;
    }

    public void Update(string videoName)
    {
        Console.WriteLine(
            $"Hi {_name}, check out the new video: {videoName}");
    }
}

// Client
public class Program
{
    public static void Main()
    {
        Channel channel =
            new Channel("Learn Coding with Lakshay");

        ISubscriber s1 = new Subscriber("Ram");
        ISubscriber s2 = new Subscriber("Shyam");

        channel.Subscribe(s1);
        channel.Subscribe(s2);

        channel.UploadVideo("Observer Pattern");

        channel.Unsubscribe(s1);

        channel.UploadVideo("Strategy Pattern");
    }
}
