


// Singleton Design Pattren implementation:
// 0nly one instance of the class should be created, thats the goal.
// We use static, member and method to achieve this since, we can't create a object of the class since the constructor will be private, so we need to use static keyword, for that.
// And why we are using, here Sealed class because we don't want any other class to inherit it: is it necessary :NO (For safety purpose only).


// THREADSAFE: BUT STILL EDGECASE, COST HEAVY.

//    private static readonly object lockObj = new object();
//  lock (lockObj)
//         {
//             if (instance == null)
//             {
//                 instance = new ThreadSafeSingleton();
//             }
//             return instance;
//         }

using System;

public sealed class Singleton {
    
    // public static Singleton _instance;
    
    // Now, this line makes it eager initialization.
    
    // Can use, Lazy instantiation, for maximum output since the object will always be created due to current, eager initialization. 
    
    // private static readonly Lazy<Singleton> _instance =
    // new Lazy<Singleton>(() => new Singleton());
    
      private static readonly Singleton _instance = new Singleton();
    
    private Singleton(){}
    
    public void Print(){
        
        Console.WriteLine("The object is created!!");
    }
    
    public static Singleton getInstance(){
        
        // Eager initilization.
        
        // if(_instance == null){
            
        //     _instance = new Singleton();
        // }
        
        return _instance;
    }
}

public class Program {
    
    public static void Main(){
        
        Singleton s1 = Singleton.getInstance();
        Singleton s2 = Singleton.getInstance();
        
        s1.Print();
        
        Console.WriteLine(object.ReferenceEquals(s1, s2));
    }
}
