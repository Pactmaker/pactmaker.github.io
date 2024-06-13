using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.CommandLine;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Wixoss.Models.Cards;
using ConcurrentCollections;

namespace Wixoss.CLI.Data;

public class ScrubCommand: Command
{
    private ILogger _logger;
    public ScrubCommand(ILoggerFactory logger): base(
        name: "scrub",
        description: "Clean up inconsistencies in the raw data"
        )
    {
        this._logger = logger.CreateLogger<ScrubCommand>();

        var dir = new Argument<DirectoryInfo>(
            name: "dir",
            description: "Input data directory"
        );

        this.Add(dir);

        this.SetHandler(async (dir) => {
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
                
                CardFixup(card);

                var options = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,
                };
                json.SetLength(0);
                await JsonSerializer.SerializeAsync(json, card, options);
                json.WriteByte(Convert.ToByte('\n'));
            });
        }, dir);
    }

    private readonly ReadOnlyDictionary<string, string> _rarityFixup = new Dictionary<string, string>() {
        {"RE", "Re"},
        {"TK", "必杀 衍生物"},
        {"强化包", "SP"},
        {"特典包", "SP"},
        {"祝福包", "SP"},
    }.AsReadOnly();

    private void CardFixup(WixossCardZhHansRaw json)
    {
        var char_type = new ConcurrentHashSet<UnicodeCategory>();

        foreach (var propertyInfo in json.GetType().GetProperties())
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                var value = (string) (propertyInfo.GetValue(json) ?? string.Empty);
                var normalized_value = new StringBuilder();
                foreach (var c in value)
                {
                    var e = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (e == UnicodeCategory.LowercaseLetter ||
                        e == UnicodeCategory.UppercaseLetter ||
                        e == UnicodeCategory.SpaceSeparator ||
                        e == UnicodeCategory.DecimalDigitNumber)
                    {
                        normalized_value.Append(c.ToString().Normalize(NormalizationForm.FormKC));
                    }
                    else
                    {
                        if (!char_type.Contains(e))
                        {
                            char_type.Add(e);
                            //Console.WriteLine($"{e}: {c}");
                        }
                        normalized_value.Append(c switch
                        {
                            '[' => '【',
                            ']' => '】',
                            ':' => '：',
                            '<' => '＜',
                            '>' => '＞',
                            '~' => '～',
                            '(' => '（',
                            ')' => '）',
                            '&' => '＆',
                            //',' => '，',
                            '#' => '＃',
                            '@' => '＠',
                            '*' => '＊',
                            _ => c,
                        });
                    }
                }
                value = normalized_value.ToString()
                                        .Replace("  ", " ")
                                        .Replace("＆amp;", "＆")
                                        .Replace("＆amp;", "＆") // WXDi-DSC03-011
                                        .Replace("＆amp;", "＆") // WXDi-DSC03-011
                                        .Replace("＆lt;", "＜")
                                        .Replace("＆gt;", "＞")
                                        .Replace("＆＃39;", "'") // WXN05-SC017
                                        .Replace("DJ.LOVIT-1stverse", "DJ．LOVIT－1stVerse") // WXN05-SC033S
                                        .Replace("DJ.LOVIT-2ndverse", "DJ．LOVIT－2ndVerse") // WXN05-SC034S
                                        .Replace("： ", "：")
                                        .Replace(" ：", "：")
                                        .Replace("】 ", "】");
                //value = Regex.Replace(value, "([a-zA-Z'][a-zA-Z])，", "$1,");
                value = string.Join(Environment.NewLine,
                            value.Split(Environment.NewLine).Select(s => s.Trim()));
                propertyInfo.SetValue(json, value);
            }
        }

        json.card_sprite_type = json.card_sprite_type.Replace(" : ", "/");
        if (json.limit.Contains("+"))
        {
            json.card_type = "分身 支援";
            json.limit = json.limit.Replace("+", string.Empty);
        }
        if (json.limit == "∞")
        {
            json.limit = $"{int.MaxValue}";
        }

        if (json.power == "12001" && json.card_no == "WXC01-SC038")
        {
            json.power = "12000";
        }

        if (json.card_lrig_type == "出自" && json.card_no == "WXC02-SC046")
        {
            json.card_lrig_type = "塔维尔";
        }

        if (json.card_lrig_type == "起" && json.card_no == "WXC02-SC090")
        {
            json.card_lrig_type = "塔维尔";
        }

        if (json.card_lrig_type == "起" && json.card_no == "WXC02-SC100")
        {
            json.card_lrig_type = "乌姆尔";
        }

        if (json.card_sprite_type == "精像：美巧" && json.card_no == "WXN02-SC245")
        {
            json.card_sprite_type = "精像/美巧";
        }

        if (json.cost == "硬币1/蓝/黑1" && (json.card_no == "WXC04-SC009" || json.card_no == "WXC04-SC009S"))
        {
            json.cost = "硬币1蓝2";
        }

        if (json.grow_price == "白1/红1/绿1" && json.card_no == "WXN01-SC00")
        {
            json.grow_price = "白1红1绿1";
        }

        json.grow_price = json.grow_price.Replace(Environment.NewLine, string.Empty);

        if (_rarityFixup.ContainsKey(json.rarity))
        {
            json.rarity = _rarityFixup[json.rarity];
        }
        json.card_effect = json.card_effect.Replace("我方主分身获得以下能力:", "我方主分身获得以下能力。");
    }
}
