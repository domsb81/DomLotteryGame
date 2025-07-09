using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;
using Microsoft.Extensions.Options;

namespace DomsLotteryGame.Services;

public class PrizeService : IPrizeService
{
    private readonly GameSettings _settings;
    private readonly IRandomProvider _randomProvider;

    public PrizeService(IRandomProvider randomProvider, IOptions<GameSettings> options)
    {
        _randomProvider = randomProvider;
        _settings = options.Value;
    }

    public PrizeDistributionResult DistributePrizes(Dictionary<int, string> ticketMap)
    {
        var result = new PrizeDistributionResult();
        int totalTickets = ticketMap.Count;
        decimal totalRevenue = totalTickets * _settings.TicketCost;

        decimal grandPrizeAmount = totalRevenue * _settings.GrandPrizePercentage;
        decimal secondTierTotal = totalRevenue * _settings.SecondTierPercentage;
        decimal thirdTierTotal = totalRevenue * _settings.ThirdTierPercentage;

        int secondTierCount = (int)Math.Round(totalTickets * 0.10);
        int thirdTierCount = (int)Math.Round(totalTickets * 0.20);

        var availableTickets = ticketMap.Keys.OrderBy(_ => _randomProvider.Next(0, 10000)).ToList();
        decimal houseProfitRemainder = 0;

        // Grand Prize
        if (availableTickets.Count > 0)
        {
            int grandTicket = availableTickets[0];
            result.GroupedWinners[grandPrizeAmount] = new List<string> { ticketMap[grandTicket] };
            result.TotalPrizeMoney += grandPrizeAmount;
            availableTickets.RemoveAt(0);
        }

        // Second Tier
        var secondTierWinners = new List<string>();
        decimal secondTierPrize = secondTierCount > 0 ? Math.Floor(secondTierTotal / secondTierCount * 100) / 100 : 0;
        decimal secondTierRemainder = secondTierTotal - (secondTierPrize * secondTierCount);
        houseProfitRemainder += secondTierRemainder;

        for (int i = 0; i < secondTierCount && availableTickets.Count > 0; i++)
        {
            secondTierWinners.Add(ticketMap[availableTickets[0]]);
            result.TotalPrizeMoney += secondTierPrize;
            availableTickets.RemoveAt(0);
        }

        if (secondTierWinners.Count > 0)
            result.GroupedWinners[secondTierPrize] = secondTierWinners;

        // Third Tier
        var thirdTierWinners = new List<string>();
        decimal thirdTierPrize = thirdTierCount > 0 ? Math.Floor(thirdTierTotal / thirdTierCount * 100) / 100 : 0;
        decimal thirdTierRemainder = thirdTierTotal - (thirdTierPrize * thirdTierCount);
        houseProfitRemainder += thirdTierRemainder;

        for (int i = 0; i < thirdTierCount && availableTickets.Count > 0; i++)
        {
            thirdTierWinners.Add(ticketMap[availableTickets[0]]);
            result.TotalPrizeMoney += thirdTierPrize;
            availableTickets.RemoveAt(0);
        }

        if (thirdTierWinners.Count > 0)
            result.GroupedWinners[thirdTierPrize] = thirdTierWinners;

        result.HouseProfit = totalRevenue - result.TotalPrizeMoney + houseProfitRemainder;

        return result;
    }
}