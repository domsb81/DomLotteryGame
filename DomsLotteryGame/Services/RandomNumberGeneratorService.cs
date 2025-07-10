using DomsLotteryGame.Interfaces;

namespace DomsLotteryGame.Services;

public class RandomNumberGeneratorService : IRandomNumberGeneratorService
{
    private readonly Random _random = new Random();

    public int Next(int min, int max)
    {
        if (min >= max)
            throw new ArgumentException($"Invalid range: min ({min}) must be less than max ({max})");

        int result = _random.Next(min, max);
        Console.WriteLine($"Generated random number: {result} in range [{min}, {max})");
        return result;
    }

    public T PickRandom<T>(List<T> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Cannot pick from an empty list.");

        int index = _random.Next(0, items.Count);
        Console.WriteLine($"Picked index {index} from list of size {items.Count}");
        return items[index];
    }

    public List<T> Shuffle<T>(List<T> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        return items.OrderBy(x => _random.Next()).ToList();
    }
}