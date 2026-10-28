namespace Roguelike.Core.Tests;

public class SchedulingSystemTests
{
    [Fact]
    public void Get_ReturnsFasterActorFirst()
    {
        var schedule = new SchedulingSystem();
        var fast = new Monster { Name = "Fast", Speed = 10 };
        var slow = new Monster { Name = "Slow", Speed = 14 };
        schedule.Add(slow);
        schedule.Add(fast);

        Actor? next = schedule.Get();

        Assert.Same(fast, next);
    }

    [Fact]
    public void Get_AdvancesTimeToActorsTurn()
    {
        var schedule = new SchedulingSystem();
        schedule.Add(new Monster { Speed = 10 });

        schedule.Get();

        Assert.Equal(10, schedule.Time);
    }

    [Fact]
    public void Get_OnEmptySchedule_ReturnsNull()
    {
        var schedule = new SchedulingSystem();

        Assert.Null(schedule.Get());
    }

    [Fact]
    public void Remove_RemovedActorIsNotReturned()
    {
        var schedule = new SchedulingSystem();
        var fast = new Monster { Name = "Fast", Speed = 10 };
        var slow = new Monster { Name = "Slow", Speed = 14 };
        schedule.Add(fast);
        schedule.Add(slow);

        schedule.Remove(fast);
        Actor? next = schedule.Get();

        Assert.Same(slow, next);
    }
}
