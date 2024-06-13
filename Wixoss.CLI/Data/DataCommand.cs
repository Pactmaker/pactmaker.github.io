using System.CommandLine;

namespace Wixoss.CLI.Data;

public class DataCommand: Command
{
    public DataCommand(ScrubCommand scrub, UniqueCommand unique): base(
        name: "data",
        description: "Performance tasks related to raw Wixoss data"
        )
    {
        Add(scrub);
        Add(unique);
    }
}
