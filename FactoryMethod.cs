
using System;

public interface IVehicleFactory {

     IVehicle CreateVehicle();
}


public interface IVehicle {
    
     void Drive();
}

public class Car : IVehicle {
    
    public void Drive(){
        
        Console.WriteLine("This is car !!");
    }
}


public class Bike : IVehicle {
    
    public void Drive() {
        
        Console.WriteLine("This is bike !!");
    }
}


public class BikeFactory : IVehicleFactory {
    
    public IVehicle CreateVehicle(){
        
        return new Bike();
    }
}

public class CarFactory : IVehicleFactory {
    
    public IVehicle CreateVehicle(){
        
        return new Car();
    }
}

public class Program {
    
    public static void Main(){
        
        // Factory method : Instead of creating a single central source, for object creation which is simple factory, create an abstract concrete class, and multiple factory classes can override it, to manipulate the implementation, which show Factory method, can have multiple Factory classes with its own creation rules.
        
        IVehicleFactory c1 = new CarFactory();
        IVehicleFactory b1 = new BikeFactory();
        
       IVehicle c2 = c1.CreateVehicle();
       IVehicle b2 = b1.CreateVehicle();
       
       c2.Drive();
       b2.Drive();
        
    }
}
