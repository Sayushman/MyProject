using MyProject.Models;
using System.Timers;

namespace MyProject.Services;

public class RealTimeDataService : IDisposable
{
    public event Action? OnDataUpdated;
    private readonly System.Timers.Timer _timer;
    private readonly List<SystemMetric> _data;
    private readonly Random _random;

    public IReadOnlyList<SystemMetric> Data => _data.AsReadOnly();

    public RealTimeDataService()
    {
        _data = new List<SystemMetric>();
        _random = new Random();
        
        // Initial data
        var now = DateTime.Now;
        for (int i = 20; i > 0; i--)
        {
            _data.Add(new SystemMetric
            {
                Timestamp = now.AddSeconds(-i),
                CpuUsage = _random.NextDouble() * 30 + 10,
                MemoryUsage = _random.NextDouble() * 20 + 40
            });
        }

        _timer = new System.Timers.Timer(1000); // update every second
        _timer.Elapsed += Timer_Elapsed;
        _timer.Start();
    }

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        var last = _data.Last();
        
        var nextCpu = last.CpuUsage + (_random.NextDouble() * 10 - 5);
        nextCpu = Math.Clamp(nextCpu, 0, 100);

        var nextMem = last.MemoryUsage + (_random.NextDouble() * 5 - 2.5);
        nextMem = Math.Clamp(nextMem, 0, 100);

        _data.Add(new SystemMetric
        {
            Timestamp = DateTime.Now,
            CpuUsage = nextCpu,
            MemoryUsage = nextMem
        });

        if (_data.Count > 20)
        {
            _data.RemoveAt(0);
        }

        OnDataUpdated?.Invoke();
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
    }
}
