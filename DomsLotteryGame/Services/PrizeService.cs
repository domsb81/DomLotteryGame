using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;
using Microsoft.Extensions.Options;

namespace DomsLotteryGame.Services;

public class PrizeService : IPrizeService
{
    private readonly GameSettings _settings;
    private readonly IRandomNumberGeneratorService _randomProvider;

    public PrizeService(IRandomNumberGeneratorService randomProvider, IOptions<GameSettings> options)
    {
        _randomProvider = randomProvider;
        _settings = options.Value;
    }

    public PrizeDistributionResult DistributePrizes(Dictionary<int, string> ticketMap)
    {
        var result = new PrizeDistributionResult();
        var totalTickets = ticketMap.Count;
        var totalRevenue = totalTickets * _settings.TicketCost;

        var availableTickets = ShuffleTickets(ticketMap.Keys.ToList());

        result.TotalPrizeMoney += AwardGrandPrize(result, ticketMap, availableTickets, totalRevenue);
        result.TotalPrizeMoney += AwardTierPrize(result, ticketMap, availableTickets, totalRevenue, _settings.SecondTierPercentage, _settings.SecondTierWinnerRatio);
        result.TotalPrizeMoney += AwardTierPrize(result, ticketMap, availableTickets, totalRevenue, _settings.ThirdTierPercentage, _settings.ThirdTierWinnerRatio);

        result.HouseProfit = totalRevenue - result.TotalPrizeMoney;

        return result;
    }

    private List<int> ShuffleTickets(List<int> ticketIds) =>
        ticketIds.OrderBy(_ => _randomProvider.Next(0, 10000)).ToList();

    private int CalculateWinnerCount(int totalTickets, decimal ratio) =>
        (int)Math.Round(totalTickets * ratio);

    private decimal AwardGrandPrize(PrizeDistributionResult result, Dictionary<int, string> ticketMap, List<int> availableTickets, decimal totalRevenue)
    {
        if (availableTickets.Count == 0) return 0;

        var prizeAmount = totalRevenue * _settings.GrandPrizePercentage;
        var winnerTicket = availableTickets[0];

        result.GroupedWinners[prizeAmount] = new List<string> { ticketMap[winnerTicket] };
        availableTickets.RemoveAt(0);

        return prizeAmount;
    }

    private decimal AwardTierPrize(
        PrizeDistributionResult result,
        Dictionary<int, string> ticketMap,
        List<int> availableTickets,
        decimal totalRevenue,
        decimal tierPercentage,
        decimal winnerRatio)
    {
        var totalTierAmount = totalRevenue * tierPercentage;
        var winnerCount = CalculateWinnerCount(ticketMap.Count, winnerRatio);

        if (winnerCount == 0 || availableTickets.Count == 0) return 0;

        var individualPrize = Math.Floor(totalTierAmount / winnerCount * 100) / 100;
        var distributedAmount = 0m;
        var winners = new List<string>();

        for (int i = 0; i < winnerCount && availableTickets.Count > 0; i++)
        {
            winners.Add(ticketMap[availableTickets[0]]);
            distributedAmount += individualPrize;
            availableTickets.RemoveAt(0);
        }

        if (winners.Count > 0)
            result.GroupedWinners[individualPrize] = winners;

        return distributedAmount;
    }
}