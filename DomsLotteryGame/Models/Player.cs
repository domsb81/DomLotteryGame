namespace DomsLotteryGame.Models;

public class Player
{
    public required string Name { get; set; }
    public decimal Balance { get; set; }
    public List<int> Tickets { get; set; } = new List<int>();
}