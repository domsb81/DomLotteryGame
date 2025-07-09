using DomsLotteryGame.Models;
namespace DomsLotteryGame.Interfaces;

public interface IPrizeService
{
    PrizeDistributionResult DistributePrizes(Dictionary<int, string> ticketMap);
}