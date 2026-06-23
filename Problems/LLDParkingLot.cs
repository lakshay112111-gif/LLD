using System;
using System.Collections.Generic;

#region ENUMS

public enum VehicleType
{
    Bike,
    Car,
    Truck
}

public enum SpotType
{
    Bike,
    Car,
    Truck
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed
}

#endregion

#region VEHICLES

public abstract class Vehicle
{
    public string VehicleNumber { get; }
    public VehicleType VehicleType { get; }

    protected Vehicle(string vehicleNumber,
                     VehicleType vehicleType)
    {
        VehicleNumber = vehicleNumber;
        VehicleType = vehicleType;
    }
}

public class Bike : Vehicle
{
    public Bike(string number)
        : base(number, VehicleType.Bike)
    {
    }
}

public class Car : Vehicle
{
    public Car(string number)
        : base(number, VehicleType.Car)
    {
    }
}

public class Truck : Vehicle
{
    public Truck(string number)
        : base(number, VehicleType.Truck)
    {
    }
}

#endregion

#region VEHICLE FACTORY

public static class VehicleFactory
{
    public static Vehicle CreateVehicle(
        string vehicleNumber,
        VehicleType type)
    {
        switch (type)
        {
            case VehicleType.Bike:
                return new Bike(vehicleNumber);

            case VehicleType.Car:
                return new Car(vehicleNumber);

            case VehicleType.Truck:
                return new Truck(vehicleNumber);

            default:
                throw new ArgumentException("Invalid Vehicle Type");
        }
    }
}

#endregion

#region PARKING SPOT

public class ParkingSpot
{
    public string SpotId { get; }

    public SpotType SpotType { get; }

    public bool IsOccupied { get; private set; }

    public Vehicle Vehicle { get; private set; }

    public ParkingSpot(
        string spotId,
        SpotType spotType)
    {
        SpotId = spotId;
        SpotType = spotType;
    }

    public bool ParkVehicle(Vehicle vehicle)
    {
        if (IsOccupied)
            return false;

        Vehicle = vehicle;
        IsOccupied = true;

        return true;
    }

    public void RemoveVehicle()
    {
        Vehicle = null;
        IsOccupied = false;
    }
}

#endregion

#region PARKING FLOOR

public class ParkingFloor
{
    public int FloorNumber { get; }

    private readonly List<ParkingSpot> _spots;

    public ParkingFloor(int floorNumber)
    {
        FloorNumber = floorNumber;
        _spots = new List<ParkingSpot>();
    }

    public void AddSpot(ParkingSpot spot)
    {
        _spots.Add(spot);
    }

    public List<ParkingSpot> GetSpots()
    {
        return _spots;
    }
}

#endregion

#region PARKING LOT

public class ParkingLot
{
    public string LotId { get; }

    private readonly List<ParkingFloor> _floors;

    public ParkingLot(string lotId)
    {
        LotId = lotId;
        _floors = new List<ParkingFloor>();
    }

    public void AddFloor(ParkingFloor floor)
    {
        _floors.Add(floor);
    }

    public List<ParkingFloor> GetFloors()
    {
        return _floors;
    }
}

#endregion

#region TICKET

public class Ticket
{
    public string TicketId { get; }

    public Vehicle Vehicle { get; }

    public ParkingSpot ParkingSpot { get; }

    public DateTime EntryTime { get; }

    public DateTime? ExitTime { get; private set; }

    public double ParkingFee { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public Ticket(
        string ticketId,
        Vehicle vehicle,
        ParkingSpot parkingSpot)
    {
        TicketId = ticketId;
        Vehicle = vehicle;
        ParkingSpot = parkingSpot;

        EntryTime = DateTime.Now;
        PaymentStatus = PaymentStatus.Pending;
    }

    public void SetExitTime()
    {
        ExitTime = DateTime.Now;
    }

    public void SetParkingFee(double fee)
    {
        ParkingFee = fee;
    }

    public void SetPaymentStatus(PaymentStatus status)
    {
        PaymentStatus = status;
    }
}

#endregion

#region TICKET FACTORY

public static class TicketFactory
{
    public static Ticket CreateTicket(
        Vehicle vehicle,
        ParkingSpot spot)
    {
        return new Ticket(
            Guid.NewGuid().ToString(),
            vehicle,
            spot);
    }
}

#endregion

#region PARKING STRATEGY

public interface IParkingStrategy
{
    ParkingSpot FindSpot(
        Vehicle vehicle,
        ParkingLot parkingLot);
}

public class NearestSpotStrategy
    : IParkingStrategy
{
    public ParkingSpot FindSpot(
        Vehicle vehicle,
        ParkingLot parkingLot)
    {
        foreach (var floor in parkingLot.GetFloors())
        {
            foreach (var spot in floor.GetSpots())
            {
                bool typeMatches =
                    spot.SpotType.ToString()
                    ==
                    vehicle.VehicleType.ToString();

                if (!spot.IsOccupied &&
                    typeMatches)
                {
                    return spot;
                }
            }
        }

        return null;
    }
}

#endregion

#region FEE STRATEGY

public interface IFeeCalculator
{
    double CalculateFee(Ticket ticket);
}

public class BikeFeeStrategy : IFeeCalculator
{
    public double CalculateFee(Ticket ticket)
    {
        double hours =
            Math.Ceiling(
                (DateTime.Now -
                 ticket.EntryTime).TotalHours);

        return hours * 10;
    }
}

public class CarFeeStrategy : IFeeCalculator
{
    public double CalculateFee(Ticket ticket)
    {
        double hours =
            Math.Ceiling(
                (DateTime.Now -
                 ticket.EntryTime).TotalHours);

        return hours * 20;
    }
}

public class TruckFeeStrategy : IFeeCalculator
{
    public double CalculateFee(Ticket ticket)
    {
        double hours =
            Math.Ceiling(
                (DateTime.Now -
                 ticket.EntryTime).TotalHours);

        return hours * 50;
    }
}

#endregion

#region PAYMENT STRATEGY

public interface IPaymentStrategy
{
    bool Pay(double amount);
}

public class UPIPayment : IPaymentStrategy
{
    public bool Pay(double amount)
    {
        Console.WriteLine(
            $"UPI Payment Successful : ₹{amount}");

        return true;
    }
}

public class CardPayment : IPaymentStrategy
{
    public bool Pay(double amount)
    {
        Console.WriteLine(
            $"Card Payment Successful : ₹{amount}");

        return true;
    }
}

public class CashPayment : IPaymentStrategy
{
    public bool Pay(double amount)
    {
        Console.WriteLine(
            $"Cash Payment Successful : ₹{amount}");

        return true;
    }
}

#endregion

#region PARKING MANAGER

public class ParkingManager
{
    private readonly ParkingLot _parkingLot;

    private readonly IParkingStrategy _parkingStrategy;

    private readonly Dictionary<string, Ticket> _tickets;

    public ParkingManager(
        ParkingLot parkingLot,
        IParkingStrategy parkingStrategy)
    {
        _parkingLot = parkingLot;
        _parkingStrategy = parkingStrategy;

        _tickets =
            new Dictionary<string, Ticket>();
    }

    public Ticket ParkVehicle(Vehicle vehicle)
    {
        ParkingSpot spot =
            _parkingStrategy.FindSpot(
                vehicle,
                _parkingLot);

        if (spot == null)
        {
            Console.WriteLine(
                "No Parking Spot Available");

            return null;
        }

        spot.ParkVehicle(vehicle);

        Ticket ticket =
            TicketFactory.CreateTicket(
                vehicle,
                spot);

        _tickets.Add(
            ticket.TicketId,
            ticket);

        Console.WriteLine(
            $"Vehicle Parked. Ticket Id : {ticket.TicketId}");

        return ticket;
    }

    public void UnparkVehicle(
        string ticketId,
        IFeeCalculator feeCalculator,
        IPaymentStrategy paymentStrategy)
    {
        if (!_tickets.ContainsKey(ticketId))
        {
            Console.WriteLine("Invalid Ticket");
            return;
        }

        Ticket ticket =
            _tickets[ticketId];

        double fee =
            feeCalculator.CalculateFee(ticket);

        Console.WriteLine(
            $"Parking Fee : ₹{fee}");

        bool paymentSuccess =
            paymentStrategy.Pay(fee);

        if (!paymentSuccess)
        {
            ticket.SetPaymentStatus(
                PaymentStatus.Failed);

            Console.WriteLine(
                "Payment Failed");

            return;
        }

        ticket.SetExitTime();
        ticket.SetParkingFee(fee);
        ticket.SetPaymentStatus(
            PaymentStatus.Paid);

        ticket.ParkingSpot.RemoveVehicle();

        Console.WriteLine(
            "Vehicle Exited Successfully");

        Console.WriteLine(
            $"Spot Freed : {ticket.ParkingSpot.SpotId}");
    }
}

#endregion

#region CLIENT

public class Program
{
    public static void Main()
    {
        ParkingLot lot =
            new ParkingLot("LOT-1");

        ParkingFloor floor1 =
            new ParkingFloor(1);

        floor1.AddSpot(
            new ParkingSpot(
                "B1",
                SpotType.Bike));

        floor1.AddSpot(
            new ParkingSpot(
                "C1",
                SpotType.Car));

        floor1.AddSpot(
            new ParkingSpot(
                "T1",
                SpotType.Truck));

        lot.AddFloor(floor1);

        IParkingStrategy parkingStrategy =
            new NearestSpotStrategy();

        ParkingManager manager =
            new ParkingManager(
                lot,
                parkingStrategy);

        Vehicle car =
            VehicleFactory.CreateVehicle(
                "DL01AB1234",
                VehicleType.Car);

        Ticket ticket =
            manager.ParkVehicle(car);

        System.Threading.Thread.Sleep(3000);

        manager.UnparkVehicle(
            ticket.TicketId,
            new CarFeeStrategy(),
            new UPIPayment());
    }
}

#endregion
