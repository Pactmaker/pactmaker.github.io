using Microsoft.Extensions.Localization;
namespace Wixoss.Models.Translations;

public class WixossPrimitive
{
    private IStringLocalizer _localizer;

    public WixossPrimitive(IStringLocalizer<WixossPrimitive> localizer)
    {
        _localizer = localizer;
    }

    public LocalizedString this[string key] => _localizer[key];
}