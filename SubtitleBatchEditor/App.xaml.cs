using Microsoft.UI.Xaml;
using SubtitleBatchEditor.Models;
using System.Collections.ObjectModel;

namespace SubtitleBatchEditor;

public partial class App : Application
{
    public static ObservableCollection<ReplaceRule> ReplacementRules { get; } = new()
    {
        new ReplaceRule("=#", "=")
    };

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var window = new MainWindow();
        window.Activate();
    }
}
