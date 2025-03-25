using Avalonia.Controls;

namespace App.Static;

public static class MainContent
{
    public static ContentControl Content = new ContentControl();

    public static void NavigateTo(UserControl page)
    {
        Content.Content = page;
    }
}