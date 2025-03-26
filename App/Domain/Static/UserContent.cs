using Avalonia.Controls;

namespace App.Domain.Static;

public static class UserContent
{
    public static ContentControl Content = new ContentControl();

    public static void NavigateTo(UserControl page)
    {
        Content.Content = page;
    }
}