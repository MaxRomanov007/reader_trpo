using System;
using System.IO;
using System.Linq;
using App.Domain.Extensions;
using App.Domain.Models;
using App.Domain.Static;
using App.Domain.Utils;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BookService;
using OfficeOpenXml;

namespace App.Pages.Admin;

public partial class CreateReportPage : UserControl
{
    private ReportFilter _filter = new();

    public CreateReportPage()
    {
        InitializeComponent();

        DataContext = _filter;
    }

    [Obsolete("Obsolete")]
    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_filter.Start > _filter.End)
        {
            ErrorTextBlock.ShowTemporaryText("Дата окончания не может быть больше даты начала");
            return;
        }

        var sfd = new SaveFileDialog
        {
            Title = "Сохранение отчета",
            Filters =
            [
                new FileDialogFilter { Name = "файл Excel", Extensions = { "xlsx" } }
            ]
        };
        var result = await sfd.ShowAsync(MainContent.MainWindow);
        if (result is not { Length: > 0 })
        {
            return;
        }

        var orders = Orders.GetCompletedOrders(_filter.Start.DateTime, _filter.End.DateTime);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();

        var sheet = package.Workbook.Worksheets.Add("Отчет");
        MyExcel.WriteSheet(sheet, orders.Select(o => new ModifiedOrder
        {
            Order = o,
            Books = o.OrderBooks.Select(ob => new ModifiedBook
            {
                Standard = ob.Book,
                InOrderCount = ob.Count
            }).ToList()
        }).ToList());

        try
        {
            await MyExcel.SaveExcelAsync(result, package);

            MessageTextBlock.ShowTemporaryText("Отчет записан");
        }
        catch (IOException)
        {
            ErrorTextBlock.ShowTemporaryText("Закройте файл перед записью");
        }
        catch (Exception ex)
        {
            ErrorTextBlock.ShowTemporaryText("Ошибка");
            Console.WriteLine(ex);
        }
    }
}