namespace SubtitleBatchEditor.Models;

public sealed class ReplaceRule
{
    public ReplaceRule(string from, string to)
    {
        From = from;
        To = to;
    }

    public string From { get; set; }

    public string To { get; set; }
}
