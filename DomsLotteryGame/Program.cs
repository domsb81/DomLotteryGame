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
        using IHost host = CreateHostBuilder(args).Build();

        var lotteryGame = host.Services.GetRequiredService<ILotteryGameService>();

        Console.Write("Enter number of rounds: ");
        if (!int.TryParse(Console.ReadLine(), out int rounds) || rounds <= 0)
        {
            Console.WriteLine("Invalid input. Defaulting to 1 round.");
            rounds = 1;
        }

        try
        {
            lotteryGame.RunGame(rounds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while running the game: {ex.Message}");
        }

        Console.WriteLine("\nGame complete. Press any key to exit...");
        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<GameSettings>(context.Configuration.GetSection("GameSettings"));

                services.AddSingleton<IPlayerService, PlayerService>();
                services.AddSingleton<ITicketService, TicketService>();
                services.AddSingleton<IPrizeService, PrizeService>();
                services.AddSingleton<IRandomNumberGeneratorService, RandomNumberGeneratorService>();
                services.AddSingleton<ILotteryGameService, LotteryGameService>();
            });
}