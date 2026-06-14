

// Simple Factory.

using System;

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

public class SimpleFactory {
    
    public static IVehicle CreateVehicle(string type){
        
        if(type == "Car") {
            
            return new Car();
        }
        
         if(type == "Bike"){
            
            return new Bike();
        }
        
        throw new ArgumentException("Invalid Vehicle type !!");
        
    }
}

public class Program {
    
    public static void Main(){
        
        // normal method, Creating too many objects from all the classes make it centralised using simple Factory, design ,create a separate class where the object creation is being handled.
        
        // Car c1 = new Car();
        // Bike b1 = new Bike();
        
        // new method, using simplefactory class.
        
        IVehicle s1 = SimpleFactory.CreateVehicle("Car");
        IVehicle s2 = SimpleFactory.CreateVehicle("Bike");
        
        s1.Drive();
        s2.Drive();
        
        
        
        
    }
}
