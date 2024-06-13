namespace Wixoss.Models.Cards.Extensions;

public static class WixossCardExtension
{
    public static string GitHost(this WixossCard card)
    {
        return card.Region switch
        {
            "WixossRegion_zhHans" => "https://github.com/Pactmaker/wixoss-data/tree/main/",
            _ => string.Empty,
        };
    }
    public static string GitEditHost(this WixossCard card)
    {
        return card.Region switch
        {
            "WixossRegion_zhHans" => "https://github.com/Pactmaker/wixoss-data/edit/main/",
            _ => string.Empty,
        };
    }
    public static string AssetHost(this WixossCard card)
    {
        return card.Region switch
        {
            "WixossRegion_zhHans" => "https://pactmaker.github.io/wixoss-data/",
            _ => string.Empty,
        };
    }

    public static string ImageURL(this WixossCard card)
    {
        return card.Region switch
        {
            "WixossRegion_zhHans" => $"assets/zh-Hans/{card.UID}.webp",
            _ => string.Empty,
        };
    }

    public static string JsonURL(this WixossCard card)
    {
        return card.Region switch
        {
            "WixossRegion_zhHans" => $"assets/zh-Hans/{card.UID}.json",
            _ => string.Empty,
        };
    }
}