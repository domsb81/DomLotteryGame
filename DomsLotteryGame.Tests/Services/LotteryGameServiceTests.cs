using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;
using DomsLotteryGame.Services;
using Moq;

namespace DomsLotteryGame.Tests.Services;

public class LotteryGameServiceTests
{
    [Fact]
    public void RunSingleRound_ShouldDistributePrizesAndUpdateStats()
    {
        // Arrange
        var mockPlayerService = new Mock<IPlayerService>();
        var mockTicketService = new Mock<ITicketService>();
        var mockPrizeService = new Mock<IPrizeService>();

        var players = new List<Player>
        {
            new Player { Name = "Player1", Tickets = new List<int> { 1, 2 } },
            new Player { Name = "Player2", Tickets = new List<int> { 3 } }
        };

        var ticketMap = new Dictionary<int, Player>
        {
            { 1, players[0] },
            { 2, players[0] },
            { 3, players[1] }
        };

        var prizeResult = new PrizeResult
        {
            WinnersByTier = new Dictionary<PrizeTier, List<(string, decimal)>>
            {
                { PrizeTier.Grand, new List<(string, decimal)> { ("Player1", 50m) } }
            },
            TotalPrizeMoney = 50m,
            HouseProfit = 10m
        };

        mockPlayerService.Setup(p => p.GeneratePlayers(It.IsAny<int>())).Returns(players);
        mockTicketService.Setup(t => t.AssignTickets(players)).Returns(ticketMap);
        mockPrizeService.Setup(p => p.DistributePrizes(It.IsAny<Dictionary<int, string>>())).Returns(prizeResult);

        var service = new LotteryGameService(mockPlayerService.Object, mockTicketService.Object, mockPrizeService.Object);

        using var input = new StringReader("2");
        using var output = new StringWriter();
        Console.SetIn(input);
        Console.SetOut(output);

        // Act
        service.RunGame(1);

        // Assert
        var consoleOutput = output.ToString();
        Assert.Contains("Player1: 2 tickets", consoleOutput);
        Assert.Contains("Player2: 1 tickets", consoleOutput);
        Assert.Contains("Grand Prize Winners:", consoleOutput);
        Assert.Contains("Player1 won £50.00", consoleOutput);
        Assert.Contains("House Profit: £10.00", consoleOutput);
    }
}