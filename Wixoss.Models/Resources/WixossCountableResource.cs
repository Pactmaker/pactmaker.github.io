namespace Wixoss.Models.Resources;

public class WixossCountableResource
{
    public string Resource { get; set; } = string.Empty;
    public int Count { get; set; }
    public override string ToString() => $"{Resource}[{Count}]";
}