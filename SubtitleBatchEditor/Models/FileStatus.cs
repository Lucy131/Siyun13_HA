namespace SubtitleBatchEditor.Models;

public sealed class FileStatus
{
    public FileStatus(string fileName, string status)
    {
        FileName = fileName;
        Status = status;
    }

    public string FileName { get; }

    public string Status { get; }
}
