using Wixoss.Models.Resources;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
namespace Wixoss.Models.Cards;

public class WixossCardZhHans: WixossCard
{
    public WixossCardZhHans(): base() {}
    public WixossCardZhHans(WixossCardZhHansRaw raw, ILogger? logger = null)
    {
        Region = "WixossRegion_zhHans";
        Effect = string.IsNullOrWhiteSpace(raw.card_effect) ? null : raw.card_effect;
        FlavorText = string.IsNullOrWhiteSpace(raw.card_lines) ? null : raw.card_lines;
        if (!string.IsNullOrWhiteSpace(raw.card_lrig_type))
        {
            LrigType = new HashSet<string>(raw.card_lrig_type.Split("/"));
        }
        Name = string.IsNullOrWhiteSpace(raw.card_name) ? null : raw.card_name;
        CardNumber = string.IsNullOrWhiteSpace(raw.card_no) ? null : raw.card_no;
        if (!string.IsNullOrWhiteSpace(raw.card_sprite_type))
        {
            Class = new HashSet<string>(raw.card_sprite_type.Split("/"));
        }
        Type = string.IsNullOrWhiteSpace(raw.card_type) ? null : raw.card_type;
        Coin = string.IsNullOrWhiteSpace(raw.coin) ? null : Convert.ToInt32(raw.coin);
        if (!string.IsNullOrWhiteSpace(raw.color))
        {
            Color = new HashSet<string>(raw.color.Split("/"));
        }
        if (!string.IsNullOrWhiteSpace(raw.cost))
        {
            Cost = new HashSet<HashSet<WixossCountableResource>>();
            var c = Regex.Split(raw.cost, @"(\d+)\n*");
            for (var i = 0; i < c.Count(); i += 2)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(c[i]))
                    {
                        continue;
                    }
                    Cost.Add(new HashSet<WixossCountableResource>(){
                        new WixossCountableResource{
                            Resource = c[i],
                            Count = Convert.ToInt32(c[i+1]),
                    }});
                }
                catch
                {
                    logger?.LogError(
                        "Parse cost '{cost}' failed. Got '{items}'",
                        raw.cost,
                        c
                    );
                    throw;
                }
            }
        }
        Format = string.IsNullOrWhiteSpace(raw.env) ? null : raw.env;
        FAQ = string.IsNullOrWhiteSpace(raw.faq) ? null : raw.faq;
        Set = string.IsNullOrWhiteSpace(raw.goods_name) ? null : raw.goods_name;
        if (!string.IsNullOrWhiteSpace(raw.grow_price))
        {
            GrowCost = new HashSet<HashSet<WixossCountableResource>>();
            if (raw.grow_price.Contains("/"))
            {
                var cost = new HashSet<WixossCountableResource>();
                var c = Regex.Split(raw.grow_price.Replace("/", string.Empty), @"(\d+)\n*");
                for (var i = 0; i < c.Count(); i += 2)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(c[i]))
                        {
                            continue;
                        }
                        cost.Add(new WixossCountableResource{
                                Resource = c[i],
                                Count = Convert.ToInt32(c[i+1]),
                        });
                    }
                    catch
                    {
                        logger?.LogError(
                            "Parse grow_price '{grow_price}' failed. Got '{items}'",
                            raw.grow_price,
                            c
                        );
                        throw;
                    }
                }
                GrowCost.Add(cost);
            }
            else
            {
                var c = Regex.Split(raw.grow_price, @"(\d+)\n*");
                for (var i = 0; i < c.Count(); i += 2)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(c[i]))
                        {
                            continue;
                        }
                        GrowCost.Add(new HashSet<WixossCountableResource>(){
                            new WixossCountableResource{
                                Resource = c[i],
                                Count = Convert.ToInt32(c[i+1]),
                        }});
                    }
                    catch
                    {
                        logger?.LogError(
                            "Parse grow_price '{grow_price}' failed. Got '{items}'",
                            raw.grow_price,
                            c
                        );
                        throw;
                    }
                }
            }
        }
        Level = string.IsNullOrWhiteSpace(raw.level) ? null : Convert.ToInt32(raw.level);
        Power = string.IsNullOrWhiteSpace(raw.power) ? null : Convert.ToInt32(raw.power);
        Rarity = string.IsNullOrWhiteSpace(raw.rarity) ? null : raw.rarity;
        Story = string.IsNullOrWhiteSpace(raw.story) ? null : raw.story;
        Team = string.IsNullOrWhiteSpace(raw.team) ? null : raw.team;
        UID = string.IsNullOrWhiteSpace(raw.uid) ? null : raw.uid;
    }
}
