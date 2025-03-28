using App.Domain.Static;
using App.Pages.Authorization;
using Avalonia.Controls;

namespace App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        MainContent.Content = WindowContent;
        MainContent.MainWindow = this;
        WindowContent.Content = new AuthorizationPage();
    }
}