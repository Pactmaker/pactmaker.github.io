using Wixoss.Models.Resources;
namespace Wixoss.Models.Cards;

public class WixossCard
{

    // Meta
    // Determines the release region
    public string? Region { get; set; }
    public string? Set { get; set; }
    public string? FAQ { get; set; }
    public string? UID { get; set; }

    // Card Top
    public string? Type { get; set; }
    public int? Level { get; set; }
    public int? Limit { get; set; }
    public HashSet<HashSet<WixossCountableResource>>? Cost { get; set; }
    public string? Name { get; set; }
    public HashSet<string>? Color { get; set; }

    // Card Tags
    public string? Story { get; set; }
    public string? Team { get; set; }
    public HashSet<string>? LrigType { get; set; }
    public HashSet<string>? Class { get; set; }

    // Card Bottom
    public string? Effect { get; set; }
    public string? FlavorText { get; set; }
    public int? Power { get; set; }
    public HashSet<HashSet<WixossCountableResource>>? GrowCost { get; set; }
    public string? CardNumber { get; set; }
    public int? Coin { get; set; }
    public string? Illustator { get; set; }
    public string? Rarity { get; set; }
    public string? Format { get; set; }
}
