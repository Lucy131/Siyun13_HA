using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SubtitleBatchEditor.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace SubtitleBatchEditor;

public sealed partial class MainWindow : Window
{
    public ObservableCollection<FileStatus> FileStatuses { get; } = new();

    private string? _selectedFolder;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnPickFolder(object sender, RoutedEventArgs e)
    {
        var picker = new FolderPicker();
        picker.FileTypeFilter.Add("*");

        var hwnd = WindowNative.GetWindowHandle(this);
        InitializeWithWindow.Initialize(picker, hwnd);

        var folder = await picker.PickSingleFolderAsync();
        if (folder is null)
        {
            return;
        }

        _selectedFolder = folder.Path;
        FolderPathTextBox.Text = _selectedFolder;
    }

    private void OnOpenSettings(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.Activate();
    }

    private void OnRunReplace(object sender, RoutedEventArgs e)
    {
        FileStatuses.Clear();

        if (string.IsNullOrWhiteSpace(_selectedFolder))
        {
            FileStatuses.Add(new FileStatus("폴더 선택 필요", "실행 실패"));
            return;
        }

        var rules = App.ReplacementRules
            .Where(rule => !string.IsNullOrWhiteSpace(rule.From))
            .ToList();

        if (!rules.Any())
        {
            FileStatuses.Add(new FileStatus("치환 규칙 없음", "실행 실패"));
            return;
        }

        var files = Directory
            .EnumerateFiles(_selectedFolder, "*.*", SearchOption.TopDirectoryOnly)
            .Where(path => path.EndsWith(".smi", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".srt", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!files.Any())
        {
            FileStatuses.Add(new FileStatus("대상 파일 없음", "실행 실패"));
            return;
        }

        foreach (var file in files)
        {
            try
            {
                var content = File.ReadAllText(file);
                var updated = ApplyReplacements(content, rules);

                if (updated != content)
                {
                    File.WriteAllText(file, updated);
                    FileStatuses.Add(new FileStatus(Path.GetFileName(file), "수정 완료"));
                }
                else
                {
                    FileStatuses.Add(new FileStatus(Path.GetFileName(file), "변경 없음"));
                }
            }
            catch (Exception ex)
            {
                FileStatuses.Add(new FileStatus(Path.GetFileName(file), $"오류: {ex.Message}"));
            }
        }
    }

    private static string ApplyReplacements(string input, IEnumerable<ReplaceRule> rules)
    {
        var updated = input;
        foreach (var rule in rules)
        {
            updated = updated.Replace(rule.From, rule.To);
        }

        return updated;
    }
}
