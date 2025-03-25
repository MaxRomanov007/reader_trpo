using App.Domain.Static;
using App.Pages;
using Avalonia.Controls;
using Avalonia.Interactivity;

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