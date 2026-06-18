using System;
using System.Collections.Generic;

// ===========================
// Vehicle Abstraction
// ===========================

public interface IVehicle
{
    string VehicleNumber { get; }
    string VehicleType { get; }
}

// ===========================
// Vehicles
// ===========================

public class Car : IVehicle
{
    public string VehicleNumber { get; }

    public string VehicleType => "Car";

    public Car(string vehicleNumber)
    {
        VehicleNumber = vehicleNumber;
    }
}

public class Bike : IVehicle
{
    public string VehicleNumber { get; }

    public string VehicleType => "Bike";

    public Bike(string vehicleNumber)
    {
        VehicleNumber = vehicleNumber;
    }
}

public class Truck : IVehicle
{
    public string VehicleNumber { get; }

    public string VehicleType => "Truck";

    public Truck(string vehicleNumber)
    {
        VehicleNumber = vehicleNumber;
    }
}

// ===========================
// Vehicle Factory
// ===========================

public class VehicleFactory
{
    public static IVehicle CreateVehicle(
        string vehicleNumber,
        string vehicleType)
    {
        if (vehicleType == "Car")
            return new Car(vehicleNumber);

        if (vehicleType == "Bike")
            return new Bike(vehicleNumber);

        if (vehicleType == "Truck")
            return new Truck(vehicleNumber);

        throw new ArgumentException("Invalid Vehicle Type");
    }
}

// ===========================
// Ticket
// ===========================

public class Ticket
{
    public string TicketId { get; }

    public IVehicle Vehicle { get; }

    public DateTime EntryTime { get; }

    public Ticket(
        string ticketId,
        IVehicle vehicle)
    {
        TicketId = ticketId;
        Vehicle = vehicle;
        EntryTime = DateTime.Now;
    }
}

// ===========================
// Ticket Generator
// ===========================

public class TicketGenerator
{
    private int _counter = 1;

    public string GenerateTicketId()
    {
        return $"T{_counter++}";
    }
}

// ===========================
// Fee Strategy
// ===========================

public interface IFeeStrategy
{
    double CalculateFee(int hours);
}

public class CarFeeStrategy : IFeeStrategy
{
    public double CalculateFee(int hours)
    {
        return hours * 20;
    }
}

public class BikeFeeStrategy : IFeeStrategy
{
    public double CalculateFee(int hours)
    {
        return hours * 10;
    }
}

public class TruckFeeStrategy : IFeeStrategy
{
    public double CalculateFee(int hours)
    {
        return hours * 50;
    }
}

// ===========================
// Fee Strategy Factory
// ===========================

public class FeeStrategyFactory
{
    public static IFeeStrategy GetStrategy(
        string vehicleType)
    {
        if (vehicleType == "Car")
            return new CarFeeStrategy();

        if (vehicleType == "Bike")
            return new BikeFeeStrategy();

        if (vehicleType == "Truck")
            return new TruckFeeStrategy();

        throw new ArgumentException("Invalid Vehicle Type");
    }
}

// ===========================
// Fee Calculator (Context)
// ===========================

public class FeeCalculator
{
    private readonly IFeeStrategy _strategy;

    public FeeCalculator(IFeeStrategy strategy)
    {
        _strategy = strategy;
    }

    public double Calculate(int hours)
    {
        return _strategy.CalculateFee(hours);
    }
}

// ===========================
// Parking Lot
// ===========================

public class ParkingLot
{
    private readonly List<Ticket> _tickets =
        new List<Ticket>();

    private readonly TicketGenerator _generator =
        new TicketGenerator();

    public Ticket ParkVehicle(
        IVehicle vehicle)
    {
        string ticketId =
            _generator.GenerateTicketId();

        Ticket ticket =
            new Ticket(ticketId, vehicle);

        _tickets.Add(ticket);

        Console.WriteLine(
            $"Vehicle Parked Successfully. Ticket: {ticketId}");

        return ticket;
    }

    public void RemoveVehicle(
        string ticketId)
    {
        Ticket ticket =
            _tickets.Find(t => t.TicketId == ticketId);

        if (ticket != null)
        {
            _tickets.Remove(ticket);

            Console.WriteLine(
                $"Vehicle Removed. Ticket: {ticketId}");
        }
        else
        {
            Console.WriteLine("Ticket Not Found");
        }
    }

    public void DisplayParkedVehicles()
    {
        Console.WriteLine("\nParked Vehicles:");

        foreach (var ticket in _tickets)
        {
            Console.WriteLine(
                $"{ticket.Vehicle.VehicleType} - {ticket.Vehicle.VehicleNumber}");
        }
    }
}

// ===========================
// Client
// ===========================

public class Program
{
    public static void Main()
    {
        ParkingLot parkingLot =
            new ParkingLot();

        // Vehicle Creation using Factory

        IVehicle car =
            VehicleFactory.CreateVehicle(
                "HR26AB1234",
                "Car");

        IVehicle bike =
            VehicleFactory.CreateVehicle(
                "DL01XY9876",
                "Bike");

        // Park Vehicles

        Ticket carTicket =
            parkingLot.ParkVehicle(car);

        Ticket bikeTicket =
            parkingLot.ParkVehicle(bike);

        parkingLot.DisplayParkedVehicles();

        Console.WriteLine();

        // Calculate Car Fee for 5 hours

        IFeeStrategy carStrategy =
            FeeStrategyFactory.GetStrategy(
                car.VehicleType);

        FeeCalculator calculator =
            new FeeCalculator(carStrategy);

        double fee =
            calculator.Calculate(5);

        Console.WriteLine(
            $"Parking Fee for Car (5 hrs): ₹{fee}");

        Console.WriteLine();

        // Remove Vehicle

        parkingLot.RemoveVehicle(
            carTicket.TicketId);

        Console.WriteLine();

        parkingLot.DisplayParkedVehicles();
    }
}
