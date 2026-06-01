namespace MyProject.Models;

public class MovementData
{
    public string Time { get; set; } = "";
    public double Inward { get; set; }
    public double Outward { get; set; }
    public double Total => Inward + Outward;
}

public class DataItem
{
    public string Category { get; set; } = "";
    public double Value { get; set; }
}

public class DockStatusData
{
    public string DockNo { get; set; } = "";
    public string VehicleNo { get; set; } = "";
    public string Activity { get; set; } = "";
    public string InTime { get; set; } = "";
    public string OutTime { get; set; } = "";
    public string Status { get; set; } = "";
}

public class VehicleTatData
{
    public string VehicleNo { get; set; } = "";
    public string EntryTime { get; set; } = "";
    public string DockIn { get; set; } = "";
    public string DockOut { get; set; } = "";
    public string ExitTime { get; set; } = "";
    public double TatHrs { get; set; }
    public string Status { get; set; } = "";
}

public class DelayedVehicleData
{
    public string VehicleNo { get; set; } = "";
    public string DelayReason { get; set; } = "";
    public double DelayHrs { get; set; }
    public string Department { get; set; } = "";
}

public class DailyTatData
{
    public string DayLabel { get; set; } = "";
    public double AvgTat { get; set; }
    public double TargetTat { get; set; }
}
