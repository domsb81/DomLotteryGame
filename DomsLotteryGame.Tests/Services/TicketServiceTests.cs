using DomsLotteryGame.Services;
using DomsLotteryGame.Models;

public class TicketServiceTests
{
    [Fact]
    public void AssignTickets_ShouldMapTicketsToPlayers()
    {
        var player = new Player { Name = "Test" };
        player.Tickets.AddRange(new List<int> { 1, 2, 3 });

        var service = new TicketService();
        var map = service.AssignTickets(new List<Player> { player });

        Assert.Equal(player, map[1]);
        Assert.Equal(player, map[2]);
        Assert.Equal(player, map[3]);
    }
}
