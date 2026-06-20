
using System;
using System.Collections.Generic;

// Observer, Pattren example, Youtube channel & Subscriber.

public interface IChannel {
    void subscribe(ISubscriber subscriber);
    void unsubscribe(ISubscriber subscriber);
    void notifySubscriber();
}

public interface ISubscriber {
    void Update();
}

// Concrete classes.

public class Channel : IChannel {
    string channelName;
    string  latestVideo;
    List<ISubscriber> subscribers = new List<ISubscriber> ();
    
    public Channel(string name){
        channelName = name;
    }
    // add subscriber.
    public void subscribe(ISubscriber subscriber){
        if(!subscribers.Contains(subscriber)){
            subscribers.Add(subscriber);
        }
    }
    
    // remove subscriber.
    public void unsubscribe(ISubscriber subscriber) {
        subscribers.Remove(subscriber);
    }
    
    public void notifySubscriber() {
        foreach(var subscriber in subscribers){
            subscriber.Update();
        }
    }
    
    public void uploadVideo(string name){
        latestVideo = name;
        notifySubscriber();
    }
    
    public string getVideoName(){
        return latestVideo;
    }
}

public class Subscriber : ISubscriber {
    public string _name;
    Channel _channel;
    
    public Subscriber(Channel channel, string name){
        _name = name;
        _channel = channel;
    }
    
    // This is called, by channel to update.
    public void Update() {
        string videoName = _channel.getVideoName();
        Console.WriteLine($"Hi, {_name}, this is {videoName}");
    }
}

public class Client {
    
    public static void Main(){
        
        Channel channel = new Channel("Learn Coding with Lakshay!");
        
        Subscriber s1 = new Subscriber(channel, "Ram");
        Subscriber s2 = new Subscriber(channel, "Shyam");
        
        channel.subscribe(s1);
        channel.subscribe(s2);
        
        channel.uploadVideo("Observer Pattren");
        
        channel.unsubscribe(s1);
        
        channel.uploadVideo("Strategy Pattren");
    
    }
}
