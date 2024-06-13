using System.Collections.Concurrent;
using System.Text.Json;
using System.IO.Compression;
using Wixoss.Models.Cards;
using Microsoft.AspNetCore.WebUtilities;
namespace Pactmaker.Shared.Shared;

public class WixossDeck
{
    public string Name { get; set; } = "Default";
    public ConcurrentDictionary<WixossCard, int> Cards { get; set; } = new();
    
    public static async Task<string> ToShareStringAsync(WixossCardList CardList, WixossDeck Deck)
    {
        if (Deck.Cards.Count() == 0)
        {
            return string.Empty;
        }
        await CardList.PopulateAsync();

        var jso = new JsonSerializerOptions();
        jso.Converters.Add(new WixossCardJsonConverter(CardList));

        using var ms = new MemoryStream();
        using (var gs = new GZipStream(ms, CompressionLevel.SmallestSize))
        {
            await JsonSerializer.SerializeAsync<WixossDeck>(gs, Deck, jso);
        }
        return WebEncoders.Base64UrlEncode(ms.ToArray());
    }

    public static async Task<WixossDeck?> FromShareStringAsync(WixossCardList CardList, string ShareString)
    {
        await CardList.PopulateAsync();

        var jso = new JsonSerializerOptions();
        jso.Converters.Add(new WixossCardJsonConverter(CardList));

        using var ms = new MemoryStream(WebEncoders.Base64UrlDecode(ShareString));
        using var gs = new GZipStream(ms, CompressionMode.Decompress);
        return await JsonSerializer.DeserializeAsync<WixossDeck>(gs, jso);
    }
}
