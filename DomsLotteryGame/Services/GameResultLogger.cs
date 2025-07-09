using System.Text.Json;
using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Models;

namespace DomsLotteryGame.Services
{
    public class GameResultLogger : IGameResultLogger
    {
        public void SaveResult(PrizeDistributionResult result, int roundNumber)
        {
            var fileName = $"GameResult_Round{roundNumber}.json";
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }
    }
}