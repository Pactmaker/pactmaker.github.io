using System.CommandLine;

namespace Wixoss.CLI.Tests;

public class TestCommand: Command
{
    public TestCommand(I18nCommand i18n): base(
        name: "test",
        description: "Test various functions"
        )
    {
        Add(i18n);
    }
}
