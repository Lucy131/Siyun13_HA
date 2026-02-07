using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SubtitleBatchEditor.Models;
using System.Collections.ObjectModel;

namespace SubtitleBatchEditor;

public sealed partial class SettingsWindow : Window
{
    public ObservableCollection<ReplaceRule> Rules => App.ReplacementRules;

    public SettingsWindow()
    {
        InitializeComponent();
    }

    private void OnAddRule(object sender, RoutedEventArgs e)
    {
        Rules.Add(new ReplaceRule(string.Empty, string.Empty));
    }

    private void OnRemoveRule(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is ReplaceRule rule)
        {
            Rules.Remove(rule);
        }
    }
}
