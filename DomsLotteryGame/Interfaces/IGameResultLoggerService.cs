using DomsLotteryGame.Models;

namespace DomsLotteryGame.Interfaces;

public interface IGameResultLoggerService
{
    void SaveResult(PrizeDistributionResult result, int roundNumber);
}