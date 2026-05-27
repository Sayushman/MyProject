namespace MyProject.Models;

public class VehicleMetrics
{
    public int TotalVehicles { get; set; }
    public double TotalVehiclesTrend { get; set; }
    
    public int InwardVehicles { get; set; }
    public double InwardVehiclesTrend { get; set; }

    public int OutwardVehicles { get; set; }
    public double OutwardVehiclesTrend { get; set; }

    public int VehiclesInYard { get; set; }
    public double VehiclesInYardTrend { get; set; }

    public int CompletedDispatch { get; set; }
    public double CompletedDispatchTrend { get; set; }

    public int PendingDispatch { get; set; }
    public double PendingDispatchTrend { get; set; }

    public double AverageTat { get; set; }
}
