using System.CommandLine;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Wixoss.Models.Translations;

namespace Wixoss.CLI.Tests;

public class I18nCommand: Command
{
    private ILogger _logger;
    private WixossPrimitive _primtive;
    public I18nCommand(ILoggerFactory logger, WixossPrimitive primtive): base(
        name: "i18n",
        description: "Test I18N output"
        )
    {
        this._logger = logger.CreateLogger<I18nCommand>();
        this._primtive = primtive;

        var locale = new Option<string>(
            name: "--locale",
            description: "Targetting locale"
        ){
            IsRequired = false
        };

        var key = new Argument<string>(
            name: "key",
            description: "Translation key"
        );

        this.Add(locale);
        this.Add(key);
        this.SetHandler((locale, key) => {
            if (!string.IsNullOrWhiteSpace(locale))
            {
                _logger.LogInformation($"No locale specified. Current culture is {CultureInfo.CurrentCulture.Name}.");
                CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(locale);
            }
            Console.WriteLine($"{_primtive[key]}");
        }, locale, key);
    }
}