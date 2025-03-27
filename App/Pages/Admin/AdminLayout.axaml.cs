using App.Domain.Static;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace App.Pages.Admin;

public partial class AdminLayout : UserControl
{
    public AdminLayout()
    {
        InitializeComponent();

        AdminContent.Content = AdminContentControl;
    }
}