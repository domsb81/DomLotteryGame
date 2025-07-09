using DomsLotteryGame.Models;

namespace DomsLotteryGame.Interfaces;

public interface IPlayerService
{
    List<Player> GeneratePlayers(int userTickets);
}