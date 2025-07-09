namespace DomsLotteryGame.Models;

public class GameStats
{
    public int TotalTicketsSold { get; set; }
    public decimal TotalPrizeMoneyDistributed { get; set; }
    public decimal TotalHouseProfit { get; set; }

    public void UpdateStats(int ticketsSold, decimal prizeMoney, decimal houseProfit)
    {
        TotalTicketsSold += ticketsSold;
        TotalPrizeMoneyDistributed += prizeMoney;
        TotalHouseProfit += houseProfit;
    }

    public override string ToString()
    {
        return $"Total Tickets Sold: {TotalTicketsSold}\n" +
               $"Total Prize Money Distributed: {TotalPrizeMoneyDistributed:C}\n" +
               $"Total House Profit: {TotalHouseProfit:C}";
    }
}