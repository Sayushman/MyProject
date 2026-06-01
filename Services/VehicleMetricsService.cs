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

    public List<DockStatusData> DockStatusLive { get; private set; }
    public List<VehicleTatData> VehicleTurnaroundTime { get; private set; }
    public List<DelayedVehicleData> TopDelayedVehicles { get; private set; }
    
    public List<DailyTatData> DailyTatPerformance { get; private set; }
    public List<DataItem> DelayAnalysis { get; private set; }
    public List<DataItem> InwardVsOutwardToday { get; private set; }
    public List<DataItem> VehicleTypeBreakup { get; private set; }

    // Footer stats
    public double SafetyCompliance { get; private set; } = 100.0;
    public string DriverInductionToday { get; private set; } = "26 / 26";
    public string DocumentsVerified { get; private set; } = "124 / 126";
    public double AverageTatHrs => CurrentMetrics.AverageTat;
    public double MaxTatHrs { get; private set; } = 3.50;
    public double OnTimePerformance { get; private set; } = 89.0;
    public int PendingVehicles => CurrentMetrics.PendingDispatch;

    public VehicleMetricsService()
    {
        _random = new Random();
        CurrentMetrics = new VehicleMetrics
        {
            TotalVehicles = 126,
            TotalVehiclesTrend = 12.0,
            InwardVehicles = 68,
            InwardVehiclesTrend = 8.0,
            OutwardVehicles = 58,
            OutwardVehiclesTrend = 15.0,
            VehiclesInYard = 24,
            VehiclesInYardTrend = -5.0,
            CompletedDispatch = 52,
            CompletedDispatchTrend = 10.0,
            PendingDispatch = 14,
            PendingDispatchTrend = -13.0,
            AverageTat = 2.55
        };

        MovementTrend = new List<MovementData>
        {
            new MovementData { Time = "06:00", Inward = 18, Outward = 0 },
            new MovementData { Time = "08:00", Inward = 20, Outward = 18 },
            new MovementData { Time = "10:00", Inward = 40, Outward = 22 },
            new MovementData { Time = "12:00", Inward = 42, Outward = 36 },
            new MovementData { Time = "14:00", Inward = 55, Outward = 41 },
            new MovementData { Time = "16:00", Inward = 60, Outward = 52 },
            new MovementData { Time = "18:00", Inward = 68, Outward = 58 }
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

        DockStatusLive = new List<DockStatusData>
        {
            new DockStatusData { DockNo = "D1", VehicleNo = "MH12AB1234", Activity = "Loading", InTime = "08:15 AM", OutTime = "10:45 AM", Status = "OCCUPIED" },
            new DockStatusData { DockNo = "D2", VehicleNo = "MH12CD5678", Activity = "Unloading", InTime = "09:05 AM", OutTime = "11:20 AM", Status = "OCCUPIED" },
            new DockStatusData { DockNo = "D3", VehicleNo = "GJ05KL8888", Activity = "Loading", InTime = "10:10 AM", OutTime = "12:30 PM", Status = "OCCUPIED" },
            new DockStatusData { DockNo = "D4", VehicleNo = "RJ19UV6789", Activity = "Loading", InTime = "11:20 AM", OutTime = "-", Status = "IN PROCESS" },
            new DockStatusData { DockNo = "D5", VehicleNo = "MH14PQ3456", Activity = "Unloading", InTime = "11:30 AM", OutTime = "-", Status = "IN PROCESS" },
            new DockStatusData { DockNo = "D6", VehicleNo = "-", Activity = "-", InTime = "-", OutTime = "-", Status = "FREE" }
        };

        VehicleTurnaroundTime = new List<VehicleTatData>
        {
            new VehicleTatData { VehicleNo = "MH12AB1234", EntryTime = "08:10 AM", DockIn = "08:15 AM", DockOut = "10:30 AM", ExitTime = "10:45 AM", TatHrs = 2.35, Status = "ON TIME" },
            new VehicleTatData { VehicleNo = "MH12CD5678", EntryTime = "09:00 AM", DockIn = "09:05 AM", DockOut = "11:10 AM", ExitTime = "11:20 AM", TatHrs = 2.20, Status = "ON TIME" },
            new VehicleTatData { VehicleNo = "GJ05KL8888", EntryTime = "10:00 AM", DockIn = "10:10 AM", DockOut = "12:20 PM", ExitTime = "12:30 PM", TatHrs = 2.30, Status = "ON TIME" },
            new VehicleTatData { VehicleNo = "MH14PQ3456", EntryTime = "11:20 AM", DockIn = "11:30 AM", DockOut = "-", ExitTime = "-", TatHrs = 0, Status = "IN PROCESS" },
            new VehicleTatData { VehicleNo = "RJ19UV6789", EntryTime = "07:45 AM", DockIn = "07:50 AM", DockOut = "10:40 AM", ExitTime = "11:15 AM", TatHrs = 3.50, Status = "DELAYED" }
        };

        TopDelayedVehicles = new List<DelayedVehicleData>
        {
            new DelayedVehicleData { VehicleNo = "RJ19UV6789", DelayReason = "Dock Unavailable", DelayHrs = 1.25, Department = "Operations" },
            new DelayedVehicleData { VehicleNo = "KA01MN4321", DelayReason = "Late Arrival", DelayHrs = 0.95, Department = "Transporter" },
            new DelayedVehicleData { VehicleNo = "GJ01AB9876", DelayReason = "Documentation", DelayHrs = 0.75, Department = "Security" },
            new DelayedVehicleData { VehicleNo = "MH12XY1111", DelayReason = "Dock Unavailable", DelayHrs = 0.60, Department = "Operations" },
            new DelayedVehicleData { VehicleNo = "MH14AB2222", DelayReason = "Late Arrival", DelayHrs = 0.50, Department = "Transporter" }
        };

        DailyTatPerformance = new List<DailyTatData>
        {
            new DailyTatData { DayLabel = "10-May", AvgTat = 2.48, TargetTat = 2.20 },
            new DailyTatData { DayLabel = "11-May", AvgTat = 2.30, TargetTat = 2.20 },
            new DailyTatData { DayLabel = "12-May", AvgTat = 2.40, TargetTat = 2.20 },
            new DailyTatData { DayLabel = "13-May", AvgTat = 2.20, TargetTat = 2.20 },
            new DailyTatData { DayLabel = "14-May", AvgTat = 2.35, TargetTat = 2.20 },
            new DailyTatData { DayLabel = "15-May", AvgTat = 2.55, TargetTat = 2.20 }
        };

        DelayAnalysis = new List<DataItem>
        {
            new DataItem { Category = "Dock Unavailable", Value = 5 },
            new DataItem { Category = "Late Arrival", Value = 3 },
            new DataItem { Category = "Documentation", Value = 2 },
            new DataItem { Category = "Security / Other", Value = 1 }
        };

        InwardVsOutwardToday = new List<DataItem>
        {
            new DataItem { Category = "Inward", Value = 68 },
            new DataItem { Category = "Outward", Value = 58 }
        };

        VehicleTypeBreakup = new List<DataItem>
        {
            new DataItem { Category = "Trailer", Value = 11 },
            new DataItem { Category = "Truck", Value = 8 },
            new DataItem { Category = "Tanker", Value = 3 },
            new DataItem { Category = "Other", Value = 2 }
        };

        _timer = new System.Timers.Timer(5000); 
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        // Simple random jitter to show live updating capability without breaking the overall chart structure.
        CurrentMetrics.TotalVehicles += _random.Next(-1, 2);
        if (CurrentMetrics.TotalVehicles < 120) CurrentMetrics.TotalVehicles = 126; // reset guard

        CurrentMetrics.AverageTat = Math.Round(CurrentMetrics.AverageTat + (_random.NextDouble() * 0.04 - 0.02), 2);
        
        OnDataUpdated?.Invoke();
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
