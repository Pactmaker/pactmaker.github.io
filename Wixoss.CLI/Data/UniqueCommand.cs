using System.Collections.Concurrent;
using System.CommandLine;
using System.Text.Json;
using ConcurrentCollections;
using Microsoft.Extensions.Logging;
using Wixoss.Models.Cards;
using Wixoss.Models.Resources;

namespace Wixoss.CLI.Data;

public class UniqueCommand: Command
{
    private ILogger _logger;
    public UniqueCommand(ILoggerFactory logger): base(
        name: "unique",
        description: "Display unique values for primitive types"
        )
    {
        this._logger = logger.CreateLogger<UniqueCommand>();
        
        var dir = new Argument<DirectoryInfo>(
            name: "dir",
            description: "Input data directory"
        );

        this.Add(dir);

        this.SetHandler(async (dir) => {
            var cards = new ConcurrentHashSet<WixossCardZhHans>();

            await Parallel.ForEachAsync(dir.GetFiles(), async (fi, token) =>
            {
                if (fi is null || fi.Extension != ".json" || fi.Name == "cards.json")
                {
                    return;
                }
                using var json = fi.Open(FileMode.Open);
                var card = await JsonSerializer.DeserializeAsync<WixossCardZhHansRaw>(json);
                if (card is null)
                {
                    return;
                }

                cards.Add(new WixossCardZhHans(card, _logger));
            });

            Console.WriteLine($"Duplicated CardNumber: {string.Join(", ", cards.Select(i => i.CardNumber)
                                            .GroupBy(i => i)
                                            .Where(g => g.Count() > 1)
                                            .Select(g => $"{g.Key}:{g.Count()}")
                                            .Order())}");
            Console.WriteLine($"Distinct LrigType: {string.Join(", ", cards.SelectMany(i => i.LrigType ?? new HashSet<string>()).Distinct().Order())}");
            Console.WriteLine($"Distinct Class: {string.Join(", ", cards.SelectMany(i => i.Class ?? new HashSet<string>()).Distinct().Order())}");
            Console.WriteLine($"Distinct Type: {string.Join(", ", cards.Select(i => i.Type).Distinct().Order())}");
            Console.WriteLine($"Distinct Coin: {string.Join(", ", cards.Select(i => i.Coin).Distinct().Order())}");
            Console.WriteLine($"Distinct Color: {string.Join(", ", cards.SelectMany(i => i.Color ?? new HashSet<string>()).Distinct().Order())}");
            Console.WriteLine($"Distinct Cost: {string.Join(", ", cards.SelectMany(i => i.Cost ?? new HashSet<HashSet<WixossCountableResource>>())
                                                                       .SelectMany(i => i)
                                                                       .Select(i => i.ToString())
                                                                       .Distinct()
                                                                       .Order())}");
            Console.WriteLine($"Distinct Format: {string.Join(", ", cards.Select(i => i.Format).Distinct().Order())}");
            Console.WriteLine($"Distinct Set: {string.Join(", ", cards.Select(i => i.Set).Distinct().Order())}");
            Console.WriteLine($"Distinct GrowCost: {string.Join(", ", cards.SelectMany(i => i.GrowCost ?? new HashSet<HashSet<WixossCountableResource>>())
                                                                           .SelectMany(i => i)
                                                                           .Select(i => i.ToString())
                                                                           .Distinct()
                                                                           .Order())}");
            Console.WriteLine($"Distinct Level: {string.Join(", ", cards.Select(i => i.Level).Distinct().Order())}");
            Console.WriteLine($"Distinct Limit: {string.Join(", ", cards.Select(i => i.Limit).Distinct().Order())}");
            Console.WriteLine($"Distinct Power: {string.Join(", ", cards.Select(i => i.Power).Distinct().Order())}");
            Console.WriteLine($"Distinct Rarity: {string.Join(", ", cards.Select(i => i.Rarity).Distinct().Order())}");
            Console.WriteLine($"Distinct Story: {string.Join(", ", cards.Select(i => i.Story).Distinct().Order())}");
            Console.WriteLine($"Distinct Team: {string.Join(", ", cards.Select(i => i.Team).Distinct().Order())}");
        }, dir);
    }
}
