namespace DomsLotteryGame.Models;

public class PrizeDistributionResult
{
    public Dictionary<decimal, List<string>> GroupedWinners { get; set; } = new();
    public decimal TotalPrizeMoney { get; set; }
    public decimal HouseProfit { get; set; }
}