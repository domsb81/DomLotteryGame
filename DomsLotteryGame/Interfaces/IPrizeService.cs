using DomsLotteryGame.Models;
namespace DomsLotteryGame.Interfaces;

public interface IPrizeService
{
    PrizeResult DistributePrizes(Dictionary<int, string> ticketMap);
}