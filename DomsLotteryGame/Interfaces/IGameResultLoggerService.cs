using DomsLotteryGame.Models;

namespace DomsLotteryGame.Interfaces;

public interface IGameResultLoggerService
{
    void SaveResult(PrizeResult result, int roundNumber);
}