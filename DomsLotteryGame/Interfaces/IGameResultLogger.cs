using DomsLotteryGame.Models;

namespace DomsLotteryGame.Interfaces
{
    public interface IGameResultLogger
    {
        void SaveResult(PrizeDistributionResult result, int roundNumber);
    }
}