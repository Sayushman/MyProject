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
