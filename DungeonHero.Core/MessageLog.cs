namespace DungeonHero.Core;

public class MessageLog
{
    public const int MaxLines = 9;

    private readonly Queue<string> _lines = new();

    public IReadOnlyCollection<string> Lines => _lines;

    public void Add(string message)
    {
        _lines.Enqueue(message);

        if (_lines.Count > MaxLines)
        {
            _lines.Dequeue();
        }
    }
}
