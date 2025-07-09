//using Microsoft.Extensions.Options;
//using Moq;
//using DomsLotteryGame.Interfaces;
//using DomsLotteryGame.Models;
//using DomsLotteryGame.Services;

//public class PrizeServiceTests
//{
//    [Fact]
//    public void DistributePrizes_ShouldReturnGroupedWinnersAndHouseProfit()
//    {
//        // Arrange
//        var settings = Options.Create(new GameSettings
//        {
//            TicketCost = 1,
//            GrandPrizePercentage = 0.5,
//            SecondTierPercentage = 0.3,
//            ThirdTierPercentage = 0.1,
//            SecondTierWinnerRatio = 0.1,
//            ThirdTierWinnerRatio = 0.2
//        });

//        var randomMock = new Mock<IRandomProvider>();
//        randomMock.Setup(r => r.PickRandom(It.IsAny<List<int>>())).Returns((List<int> list) => list[0]);
//        randomMock.Setup(r => r.Shuffle(It.IsAny<List<int>>())).Returns((List<int> list) => list);

//        var prizeService = new PrizeService(settings, randomMock.Object);

//        var players = new List<Player>
//        {
//            new Player { Name = "Alice", Tickets = new List<int> { 1, 2 } },
//            new Player { Name = "Bob", Tickets = new List<int> { 3, 4 } },
//            new Player { Name = "Charlie", Tickets = new List<int> { 5, 6 } }
//        };

//        var ticketMap = players.SelectMany(p => p.Tickets.Select(t => new { Ticket = t, Player = p }))
//                               .ToDictionary(x => x.Ticket, x => x.Player);

//        // Act
//        var result = prizeService.DistributePrizes(ticketMap);

//        // Assert
//        Assert.NotNull(result);
//        Assert.True(result.GroupedWinners.Count > 0);
//        Assert.True(result.HouseProfit >= 0);

//        int totalPrizeDistributed = result.GroupedWinners.Sum(g => g.Key * g.Value.Count);
//        int expectedRevenue = ticketMap.Count * settings.Value.TicketCost;
//        Assert.Equal(expectedRevenue - result.HouseProfit, totalPrizeDistributed);
//    }
//}