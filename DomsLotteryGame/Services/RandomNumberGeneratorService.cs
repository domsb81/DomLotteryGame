using DomsLotteryGame.Interfaces;

namespace DomsLotteryGame.Services;

public class RandomNumberGeneratorService : IRandomNumberGeneratorService
{
    private readonly Random _random = new Random();

    public int Next(int min, int max) => _random.Next(min, max + 1);

    public T PickRandom<T>(List<T> items) => items[_random.Next(items.Count)];

    public List<T> Shuffle<T>(List<T> items) => items.OrderBy(x => _random.Next()).ToList();
}