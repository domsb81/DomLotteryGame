using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;
using DomsLotteryGame.Models;

namespace DomsLotteryGame.Services
{
    public class TicketService : ITicketService
    {
        public Dictionary<int, Player> AssignTickets(List<Player> players)
        {
            var ticketMap = new Dictionary<int, Player>();
            foreach (var player in players)
            {
                foreach (var ticket in player.Tickets)
                {
                    ticketMap[ticket] = player;
                }
            }
            return ticketMap;
        }
    }
}
