using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;
using Microsoft.Extensions.Options;

namespace DomsLotteryGame.Services;

public class PlayerService : IPlayerService
{
    private readonly GameSettings _settings;
    private readonly IRandomNumberGeneratorService _random;

    public PlayerService(IOptions<GameSettings> options, IRandomNumberGeneratorService random)
    {
        _settings = options.Value;
        _random = random;
    }

    public List<Player> GeneratePlayers(int userTickets)
    {
        var players = new List<Player>();
        var user = new Player { Name = "Player 1", Balance = _settings.StartingBalance };
        int affordable = Math.Min(userTickets, user.Balance / _settings.TicketCost);
        for (int i = 0; i < affordable; i++)
        {
            user.Tickets.Add(i + 1);
            user.Balance -= _settings.TicketCost;
        }
        players.Add(user);

        int totalPlayers = _random.Next(_settings.MinPlayers - 1, _settings.MaxPlayers - 1);
        int ticketId = user.Tickets.Count + 1;

        for (int i = 2; i <= totalPlayers + 1; i++)
        {
            var cpu = new Player { Name = $"Player {i}", Balance = _settings.StartingBalance };
            int cpuTickets = _random.Next(_settings.MinTickets, _settings.MaxTickets);
            int affordableCpu = Math.Min(cpuTickets, cpu.Balance / _settings.TicketCost);
            for (int j = 0; j < affordableCpu; j++)
            {
                cpu.Tickets.Add(ticketId++);
                cpu.Balance -= _settings.TicketCost;
            }
            players.Add(cpu);
        }

        return players;
    }
}