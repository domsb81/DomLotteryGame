using DomsLotteryGame.Models;
using DomsLotteryGame.Models;

namespace DomsLotteryGame.Interfaces
{
    public interface ITicketService
    {
        Dictionary<int, Player> AssignTickets(List<Player> players);
    }
}
