namespace DungeonHero.Core;

public class SchedulingSystem
{
    private readonly PriorityQueue<Actor, int> _queue = new();

    public int Time { get; private set; }

    public void Add(Actor actor)
    {
        _queue.Enqueue(actor, Time + actor.Speed);
    }

    public Actor? Get()
    {
        if (!_queue.TryDequeue(out Actor? actor, out int time))
        {
            return null;
        }

        Time = time;
        return actor;
    }

    public void Remove(Actor actor)
    {
        _queue.Remove(actor, out _, out _);
    }

    public void Clear()
    {
        _queue.Clear();
        Time = 0;
    }
}
