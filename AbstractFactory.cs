
using System;

public interface ICar {
     void Drive();
}

public interface IBike {
     void Ride();   
}

public class RegularCar : ICar {
    
    public void Drive(){
        
        Console.WriteLine("This is Regular Car!!");
    }
}

public class RegularBike : IBike {
    
    public void Ride() {
        
        Console.WriteLine("This is Regular Bike!!");
    }
}

public class LuxuryCar : ICar {
    
    public void Drive(){
        
        Console.WriteLine("This is Luxury Car!!");
    }
}

public class LuxuryBike : IBike {
    
    public void Ride() {
        
        Console.WriteLine("This is Luxury Bike!!");
    }
}

public interface IVehicleFactory {
    
    public ICar CreateCar();
    public IBike CreateBike();
}

public class RegularFactory : IVehicleFactory {
    
    public ICar CreateCar() {
        
        return new RegularCar();
    }
    
    public IBike CreateBike() {
        
        return new RegularBike();
    }
}


public class LuxuryFactory : IVehicleFactory {
    
    public ICar CreateCar() {
        
        return new LuxuryCar();
    }
    
    public IBike CreateBike() {
        
        return new LuxuryBike();
    }
}

public class Program {
    
    public static void Main(){
        
        // Regular Factory.
        IVehicleFactory v1 = new RegularFactory();
        ICar c1 = v1.CreateCar();
        IBike b1 = v1.CreateBike();
    
        c1.Drive();
        b1.Ride();
        
         // Luxury Factory.
        IVehicleFactory v2 = new LuxuryFactory();
        ICar c2 = v2.CreateCar();
        IBike b2 = v2.CreateBike();
        
        c2.Drive();
        b2.Ride();
    }
}
