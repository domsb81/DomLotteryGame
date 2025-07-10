namespace DomsLotteryGame.Models;

public class GameSettings
{
    public decimal StartingBalance { get; set; }
    public decimal TicketCost { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int MinTickets { get; set; }
    public int MaxTickets { get; set; }
    public decimal GrandPrizePercentage { get; set; }
    public decimal SecondTierPercentage { get; set; }
    public decimal SecondPrizePoolPercentage { get; set; }
    public decimal ThirdTierPercentage { get; set; }
    public decimal ThirdPrizePoolPercentage { get; set; }
}