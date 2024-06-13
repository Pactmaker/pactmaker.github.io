using ConcurrentCollections;
using Blazored.LocalStorage;
using System.Text.Json.Serialization;
namespace Pactmaker.Shared.Shared;

public class WixossDeckList
{
    private WixossCardList CardList;
    private ILocalStorageService LocalStorage;
    
    [JsonIgnore]
    public WixossDeck SelectedDeck { get; set; } = null!;
    public ConcurrentHashSet<WixossDeck> Decks { get; set; } = new();

    public WixossDeckList(WixossCardList CardList, ILocalStorageService LocalStorage): base()
    {
        this.CardList = CardList;
        this.LocalStorage = LocalStorage;
    }

    public async Task PopulateAsync()
    {
        if (this.Decks.Count() != 0)
        {
            return;
        }

        await CardList.PopulateAsync();

        var decks = await LocalStorage.GetItemAsync<ConcurrentHashSet<WixossDeck>>("decks");
        if (decks is null)
        {
            this.Decks.Add(new WixossDeck());
            await SaveAsync(LocalStorage);
        }
        else
        {
            Parallel.ForEach(decks, i =>
            {
                this.Decks.Add(i);
            });
        }
        SelectedDeck = this.Decks.First();
    }

    public async Task SaveAsync(ILocalStorageService LocalStorage)
    {
        await LocalStorage.SetItemAsync("decks", this.Decks);
    }
}