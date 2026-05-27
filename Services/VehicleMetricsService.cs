using MyProject.Models;
using System.Timers;

namespace MyProject.Services;

public class VehicleMetricsService : IDisposable
{
    public event Action? OnDataUpdated;
    private readonly System.Timers.Timer _timer;
    private readonly Random _random;

    public VehicleMetrics CurrentMetrics { get; private set; }
    
    public List<MovementData> MovementTrend { get; private set; }
    public List<DataItem> DockUtilization { get; private set; }
    public List<DataItem> YardStatus { get; private set; }

    public VehicleMetricsService()
    {
        _random = new Random();
        CurrentMetrics = new VehicleMetrics
        {
            TotalVehicles = 26,
            TotalVehiclesTrend = 12.0,
            InwardVehicles = 68,
            InwardVehiclesTrend = 8.0,
            OutwardVehicles = 58,
            OutwardVehiclesTrend = 15.0,
            VehiclesInYard = 24,
            VehiclesInYardTrend = -5.0,
            CompletedDispatch = 52,
            CompletedDispatchTrend = -10.0,
            PendingDispatch = 14,
            PendingDispatchTrend = -13.0,
            AverageTat = 2.55
        };

        MovementTrend = new List<MovementData>
        {
            new MovementData { Time = "06:00", Inward = 10, Outward = 8 },
            new MovementData { Time = "08:00", Inward = 22, Outward = 16 },
            new MovementData { Time = "10:00", Inward = 40, Outward = 22 },
            new MovementData { Time = "12:00", Inward = 55, Outward = 35 },
            new MovementData { Time = "14:00", Inward = 65, Outward = 47 },
            new MovementData { Time = "16:00", Inward = 78, Outward = 58 },
            new MovementData { Time = "18:00", Inward = 88, Outward = 68 }
        };

        DockUtilization = new List<DataItem>
        {
            new DataItem { Category = "Occupied", Value = 5 },
            new DataItem { Category = "Free", Value = 4 },
            new DataItem { Category = "In Process", Value = 2 },
            new DataItem { Category = "Maintenance", Value = 1 }
        };

        YardStatus = new List<DataItem>
        {
            new DataItem { Category = "Waiting for Dock", Value = 9 },
            new DataItem { Category = "At Dock (Loading)", Value = 7 },
            new DataItem { Category = "At Dock (Unloading)", Value = 5 },
            new DataItem { Category = "Waiting for Exit", Value = 3 }
        };

        // Update every 2 seconds to simulate real-time live data
        _timer = new System.Timers.Timer(4000); 
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        // Simulate minor variations in data for real-time effect
        CurrentMetrics.TotalVehicles += _random.Next(2, 3);
        CurrentMetrics.TotalVehiclesTrend = Math.Round(CurrentMetrics.TotalVehiclesTrend + (_random.NextDouble() * 2 - 1), 1);
        
        CurrentMetrics.InwardVehicles += _random.Next(-1, 2);
        CurrentMetrics.InwardVehiclesTrend = Math.Round(CurrentMetrics.InwardVehiclesTrend + (_random.NextDouble() * 2 - 1), 1);

        CurrentMetrics.OutwardVehicles += _random.Next(-1, 2);
        CurrentMetrics.OutwardVehiclesTrend = Math.Round(CurrentMetrics.OutwardVehiclesTrend + (_random.NextDouble() * 2 - 1), 1);

        CurrentMetrics.VehiclesInYard += _random.Next(-1, 2);
        CurrentMetrics.VehiclesInYardTrend = Math.Round(CurrentMetrics.VehiclesInYardTrend + (_random.NextDouble() * 2 - 1), 1);

        CurrentMetrics.CompletedDispatch += _random.Next(-1, 2);
        CurrentMetrics.CompletedDispatchTrend = Math.Round(CurrentMetrics.CompletedDispatchTrend + (_random.NextDouble() * 2 - 1), 1);

        CurrentMetrics.PendingDispatch += _random.Next(-1, 2);
        CurrentMetrics.PendingDispatchTrend = Math.Round(CurrentMetrics.PendingDispatchTrend + (_random.NextDouble() * 2 - 1), 1);

        CurrentMetrics.AverageTat = Math.Round(CurrentMetrics.AverageTat + (_random.NextDouble() * 0.1 - 0.05), 2);

        OnDataUpdated?.Invoke();
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
