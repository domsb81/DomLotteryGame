namespace DomsLotteryGame.Interfaces;

public interface IRandomNumberGeneratorService
{
    int Next(int min, int max);
    T PickRandom<T>(List<T> items);
    List<T> Shuffle<T>(List<T> items);
}