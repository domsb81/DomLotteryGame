using Moq;
using Microsoft.Extensions.Options;
using DomsLotteryGame.Models;
using DomsLotteryGame.Services;
using DomsLotteryGame.Interfaces;

namespace DomsLotteryGame.Tests.Services
{
    public class PrizeServiceTests
    {
        private PrizeService CreateService(GameSettings settings, Queue<int> randomSequence)
        {
            var mockOptions = new Mock<IOptions<GameSettings>>();
            mockOptions.Setup(o => o.Value).Returns(settings);

            var mockRandom = new Mock<IRandomNumberGeneratorService>();
            mockRandom.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                      .Returns(() => randomSequence.Dequeue());

            return new PrizeService(mockOptions.Object, mockRandom.Object);
        }

        [Fact]
        public void DistributePrizes_ShouldDistributeCorrectly_WithNormalInput()
        {
            var settings = new GameSettings
            {
                TicketCost = 10,
                GrandPrizePercentage = 0.5m,
                SecondTierPercentage = 0.2m,
                SecondPrizePoolPercentage = 0.3m,
                ThirdTierPercentage = 0.3m,
                ThirdPrizePoolPercentage = 0.2m
            };

            var ticketMap = new Dictionary<int, string>
            {
                { 0, "Player0" },
                { 1, "Player1" },
                { 2, "Player2" },
                { 3, "Player1" },
                { 4, "Player2" },
                { 5, "Player1" },
                { 6, "Player2" },
                { 7, "Player0" },
                { 8, "Player1" },
                { 9, "Player2" }
            };

            var randomSequence = new Queue<int>(new[] { 0, 4, 1, 2, 3 });
            var service = CreateService(settings, randomSequence);

            var result = service.DistributePrizes(ticketMap);

            Assert.Equal(2, result.WinnersByTier.Count);
            Assert.True(result.TotalPrizeMoney > 0);
            Assert.True(result.HouseProfit >= 0);
        }

        [Fact]
        public void DistributePrizes_ShouldThrowException_WhenTicketMapIsEmpty()
        {
            var settings = new GameSettings { TicketCost = 10 };
            var ticketMap = new Dictionary<int, string>();
            var randomSequence = new Queue<int>();
            var service = CreateService(settings, randomSequence);

            Assert.Throws<InvalidOperationException>(() => service.DistributePrizes(ticketMap));
        }

        [Fact]
        public void DistributePrizes_ShouldHandleSingleTicket()
        {
            var settings = new GameSettings
            {
                TicketCost = 10,
                GrandPrizePercentage = 0.5m,
                SecondTierPercentage = 0.2m,
                SecondPrizePoolPercentage = 0.3m,
                ThirdTierPercentage = 0.3m,
                ThirdPrizePoolPercentage = 0.2m
            };

            var ticketMap = new Dictionary<int, string>
            {
                { 0, "SoloPlayer" }
            };

            var randomSequence = new Queue<int>(new[] { 0 });
            var service = CreateService(settings, randomSequence);

            var result = service.DistributePrizes(ticketMap);

            Assert.Single(result.WinnersByTier);
            Assert.Contains(PrizeTier.Grand, result.WinnersByTier.Keys);
            Assert.Equal(5.0m, result.WinnersByTier[PrizeTier.Grand][0].Item2);
        }
    }
}