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
        int ticketId = 1;

        var user = CreatePlayer("Player 1", userTickets, ref ticketId);
        players.Add(user);

        int totalPlayers = _random.Next(_settings.MinPlayers - 1, _settings.MaxPlayers - 1);
        for (int i = 2; i <= totalPlayers + 1; i++)
        {
            int cpuTickets = _random.Next(_settings.MinTickets, _settings.MaxTickets);
            var cpu = CreatePlayer($"Player {i}", cpuTickets, ref ticketId);
            players.Add(cpu);
        }

        return players;
    }

    private Player CreatePlayer(string name, decimal requestedTickets, ref int ticketId)
    {
        var player = new Player
        {
            Name = name,
            Balance = _settings.StartingBalance
        };

        decimal affordable = Math.Min(requestedTickets, player.Balance / _settings.TicketCost);

        for (int i = 0; i < affordable; i++)
        {
            player.Tickets.Add(ticketId++);
            player.Balance -= _settings.TicketCost;
        }

        return player;
    }
}