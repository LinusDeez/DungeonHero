using DungeonHero.Core;

namespace DungeonHero.Core.Tests;

public class MessageLogTests
{
    [Fact]
    public void Add_StoresMessage()
    {
        var log = new MessageLog();

        log.Add("Hello");

        Assert.Single(log.Lines);
        Assert.Equal("Hello", log.Lines.First());
    }

    [Fact]
    public void Add_MoreThanMaxLines_RemovesOldest()
    {
        var log = new MessageLog();

        for (int i = 1; i <= MessageLog.MaxLines + 1; i++)
        {
            log.Add($"Message {i}");
        }

        Assert.Equal(MessageLog.MaxLines, log.Lines.Count);
        Assert.Equal("Message 2", log.Lines.First());
    }
}
