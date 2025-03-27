using Avalonia.Controls;

namespace App.Domain.Static;

public static class MainContent
{
    public static ContentControl Content = new();
    public static Window MainWindow = new();

    public static void NavigateTo(UserControl page)
    {
        Content.Content = page;
    }
}