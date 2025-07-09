using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DomsLotteryGame.Models;
using DomsLotteryGame.Interfaces;
using DomsLotteryGame.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<GameSettings>(context.Configuration.GetSection("GameSettings"));

        // Register services
        services.AddSingleton<IPlayerService, PlayerService>();
        services.AddSingleton<ITicketService, TicketService>();
        services.AddSingleton<IPrizeService, PrizeService>();
        services.AddSingleton<IRandomProvider, RandomProvider>();
        services.AddSingleton<ILotteryGame, LotteryGame>();
    })
    .Build();

        var lotteryGame = host.Services.GetRequiredService<ILotteryGame>();

        Console.Write("Enter number of rounds: ");
        int rounds = int.Parse(Console.ReadLine() ?? "1");

        lotteryGame.RunGame(rounds);
    }
}