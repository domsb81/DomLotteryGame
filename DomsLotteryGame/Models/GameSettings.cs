namespace DomsLotteryGame.Models;

public class GameSettings
{
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int MinTickets { get; set; }
    public int MaxTickets { get; set; }
    public int StartingBalance { get; set; }
    public int TicketCost { get; set; }
    public decimal GrandPrizePercentage { get; set; }
    public decimal SecondTierPercentage { get; set; }
    public decimal ThirdTierPercentage { get; set; }
    public decimal SecondTierWinnerRatio { get; set; }
    public decimal ThirdTierWinnerRatio { get; set; }
}