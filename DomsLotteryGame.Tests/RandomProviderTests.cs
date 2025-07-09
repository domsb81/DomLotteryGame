using Xunit;
using DomsLotteryGame.Services;
using System.Collections.Generic;

public class RandomProviderTests
{
    [Fact]
    public void Next_ShouldReturnWithinRange()
    {
        var random = new RandomNumberGeneratorService();
        for (int i = 0; i < 100; i++)
        {
            int value = random.Next(1, 5);
            Assert.InRange(value, 1, 5);
        }
    }

    [Fact]
    public void PickRandom_ShouldReturnItemFromList()
    {
        var random = new RandomNumberGeneratorService();
        var list = new List<string> { "A", "B", "C" };
        var result = random.PickRandom(list);
        Assert.Contains(result, list);
    }

    [Fact]
    public void Shuffle_ShouldReturnSameItems()
    {
        var random = new RandomNumberGeneratorService();
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var shuffled = random.Shuffle(list);
        Assert.Equal(list.OrderBy(x => x).ToList(), shuffled.OrderBy(x => x).ToList());
    }
}
