using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;

namespace DomsLotteryGame.Services;

public class LotteryGameService : ILotteryGameService
{
    private readonly IPlayerService _playerService;
    private readonly ITicketService _ticketService;
    private readonly IPrizeService _prizeService;
    private readonly GameStats _gameStats;

    public LotteryGameService(IPlayerService playerService, ITicketService ticketService, IPrizeService prizeService)
    {
        _playerService = playerService;
        _ticketService = ticketService;
        _prizeService = prizeService;
        _gameStats = new GameStats();
    }

    public void RunGame(int rounds)
    {
        for (int i = 1; i <= rounds; i++)
        {
            Console.WriteLine($"--- Round {i} ---");
            RunSingleRound();
        }

        Console.WriteLine("\n=== Cumulative Game Stats ===");
        Console.WriteLine(_gameStats);
    }

    private void RunSingleRound()
    {
        Console.Write("Enter number of tickets to purchase (1-10): ");
        int userTickets = int.Parse(Console.ReadLine() ?? "1");

        var players = _playerService.GeneratePlayers(userTickets);

        Console.WriteLine("\nTicket Purchases:");
        foreach (var p in players)
        {
            Console.WriteLine($"{p.Name}: {p.Tickets.Count} tickets");
        }

        var ticketMap = _ticketService.AssignTickets(players)
         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name);

        var result = _prizeService.DistributePrizes(ticketMap);

        Console.WriteLine("\nWinners Grouped by Prize Tier:");
        foreach (var tier in result.WinnersByTier.OrderBy(t => t.Key))
        {
            Console.WriteLine($"\n{tier.Key} Prize Winners:");
            foreach (var (name, prize) in tier.Value.Distinct())
            {
                Console.WriteLine($"- {name} won £{prize:F2}");
            }
        }

        Console.WriteLine($"\nHouse Profit: £{result.HouseProfit}");

        // Update cumulative stats
        int totalTickets = ticketMap.Count;
        _gameStats.UpdateStats(totalTickets, result.TotalPrizeMoney, result.HouseProfit);
    }
}