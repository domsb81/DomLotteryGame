using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;
using Microsoft.Extensions.Options;

namespace DomsLotteryGame.Services;

public class PrizeService : IPrizeService
{
    private readonly GameSettings _settings;
    private readonly IRandomNumberGeneratorService _random;

    public PrizeService(IOptions<GameSettings> options, IRandomNumberGeneratorService random)
    {
        _settings = options.Value;
        _random = random;
    }

    public PrizeResult DistributePrizes(Dictionary<int, string> ticketMap)
    {
        if (ticketMap == null || ticketMap.Count == 0)
            throw new InvalidOperationException("No tickets available for prize distribution.");

        var result = new PrizeResult();
        var allTickets = new List<int>(ticketMap.Keys);
        var usedTickets = new HashSet<int>();
        var playerWinners = new HashSet<string>();
        decimal totalRevenue = ticketMap.Count * _settings.TicketCost;

        // Grand Prize
        var grandPool = allTickets.Except(usedTickets).ToList();
        if (grandPool.Count > 0)
        {
            int grandWinner = DrawTicket(grandPool);
            decimal grandPrize = Math.Floor(totalRevenue * _settings.GrandPrizePercentage * 100) / 100;
            string player = ticketMap[grandWinner];
            AddWinner(result, PrizeTier.Grand, grandPrize, player);
            usedTickets.Add(grandWinner);
            playerWinners.Add(player);
        }

        // Second Tier
        int secondCount = (int)Math.Round(ticketMap.Count * _settings.SecondTierPercentage);
        decimal secondPrizePool = totalRevenue * _settings.SecondPrizePoolPercentage;
        var secondPool = allTickets
            .Where(t => !usedTickets.Contains(t) && !playerWinners.Contains(ticketMap[t]))
            .ToList();
        DistributeTier(result, PrizeTier.Second, secondCount, secondPrizePool, secondPool, usedTickets, playerWinners, ticketMap);

        // Third Tier
        int thirdCount = (int)Math.Round(ticketMap.Count * _settings.ThirdTierPercentage);
        decimal thirdPrizePool = totalRevenue * _settings.ThirdPrizePoolPercentage;
        var thirdPool = allTickets
            .Where(t => !usedTickets.Contains(t) && !playerWinners.Contains(ticketMap[t]))
            .ToList();
        DistributeTier(result, PrizeTier.Third, thirdCount, thirdPrizePool, thirdPool, usedTickets, playerWinners, ticketMap);

        result.TotalPrizeMoney = result.WinnersByTier
            .SelectMany(kvp => kvp.Value)
            .Sum(entry => entry.Prize);

        result.HouseProfit = Math.Round(totalRevenue - result.TotalPrizeMoney, 2);
        return result;
    }

    private void DistributeTier(
        PrizeResult result,
        PrizeTier tier,
        int count,
        decimal pool,
        List<int> eligible,
        HashSet<int> used,
        HashSet<string> playerWinners,
        Dictionary<int, string> map)
    {
        if (count == 0 || eligible.Count == 0) return;

        int actualCount = Math.Min(count, eligible.Count);
        decimal prize = Math.Floor(pool / actualCount * 100) / 100;

        for (int i = 0; i < actualCount && eligible.Count > 0; i++)
        {
            int winner = DrawTicket(eligible);
            string player = map[winner];
            AddWinner(result, tier, prize, player);
            used.Add(winner);
            playerWinners.Add(player);
            eligible.Remove(winner);
        }
    }

    private int DrawTicket(List<int> pool)
    {
        if (pool == null)
            throw new ArgumentNullException(nameof(pool), "Ticket pool cannot be null.");

        if (pool.Count == 0)
            throw new InvalidOperationException("Cannot draw from an empty ticket pool.");

        int index = _random.Next(0, pool.Count);

        if (index < 0 || index >= pool.Count)
            throw new IndexOutOfRangeException($"Random index {index} is out of bounds for pool size {pool.Count}.");

        return pool[index];
    }

    private void AddWinner(PrizeResult result, PrizeTier tier, decimal prize, string name)
    {
        if (!result.WinnersByTier.ContainsKey(tier))
            result.WinnersByTier[tier] = new List<(string, decimal)>();

        result.WinnersByTier[tier].Add((name, prize));
    }
}