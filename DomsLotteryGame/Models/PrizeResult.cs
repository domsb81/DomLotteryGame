namespace DomsLotteryGame.Models;

public class PrizeResult
{
    public Dictionary<PrizeTier, List<(string PlayerName, decimal Prize)>> WinnersByTier { get; set; } = new();
    public decimal TotalPrizeMoney { get; set; }
    public decimal HouseProfit { get; set; }
}