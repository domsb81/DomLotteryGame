using DomsLotteryGame.Services;
using DomsLotteryGame.Models;
using Microsoft.Extensions.Options;
using Moq;
using DomsLotteryGame.Interfaces;

public class PlayerServiceTests
{
    [Fact]
    public void GeneratePlayers_ShouldCreateCorrectNumberOfPlayers()
    {
        var settings = Options.Create(new GameSettings
        {
            MinPlayers = 10,
            MaxPlayers = 10,
            StartingBalance = 10,
            TicketCost = 1,
            MinTickets = 1,
            MaxTickets = 50
        });

        var randomMock = new Mock<IRandomProvider>();
        randomMock.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(2);

        var service = new PlayerService(settings, randomMock.Object);
        var players = service.GeneratePlayers(10);

        Assert.True(players.Count == 10);
        Assert.Equal("Player 1", players[0].Name);
    }
}
