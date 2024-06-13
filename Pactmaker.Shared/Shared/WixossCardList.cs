using ConcurrentCollections;
using Wixoss.Models.Cards;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Pactmaker.Shared.Shared;

public class WixossCardList
{
    private HttpClient Http;
    public WixossCardList(HttpClient Http): base() => this.Http = Http;
    public ConcurrentHashSet<WixossCard> Cards { get; set; } = new();

    public async Task<bool> PopulateAsync()
    {
        if (this.Cards.Count() != 0)
        {
            return true;
        }

        var RawCardList = await Http.GetFromJsonAsync<ConcurrentHashSet<WixossCardZhHansRaw>>("assets/zh-Hans/cards.json");
        if (RawCardList is null)
        {
            return false;
        }

        var ret = true;
        Parallel.ForEach(RawCardList, i =>
        {
            ret = ret && this.Cards.Add(new WixossCardZhHans(i));
        });
        return ret;
    }
}

public class WixossCardJsonConverter : JsonConverter<WixossCard>
{
    private WixossCardList CardList;

    public WixossCardJsonConverter(WixossCardList CardList): base()
    {
        this.CardList = CardList;
    }

    public override WixossCard Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
        {
            var uid = reader.GetString();
            if (string.IsNullOrWhiteSpace(uid))
            {
                return new();
            }
            else
            {
                return CardList.Cards.Where(i => i.UID == uid).SingleOrDefault() ?? new();
            }
        }

    public override WixossCard ReadAsPropertyName(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
        {
            var uid = reader.GetString();
            if (string.IsNullOrWhiteSpace(uid))
            {
                return new();
            }
            else
            {
                return CardList.Cards.Where(i => i.UID == uid).SingleOrDefault() ?? new();
            }
        }

    public override void Write(
        Utf8JsonWriter writer,
        WixossCard card,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(card.UID);

    public override void WriteAsPropertyName(
        Utf8JsonWriter writer,
        WixossCard card,
        JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(card.UID))
        {
            throw new ArgumentNullException();
        }
        writer.WritePropertyName(card.UID);
    }
}