using App.Domain.Static;
using App.Pages;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AuthorizationPage = App.Pages.Authorization.AuthorizationPage;

namespace App;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        
        MainContent.Content = WindowContent;
        WindowContent.Content = new AuthorizationPage();
    }
}