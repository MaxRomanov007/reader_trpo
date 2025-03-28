using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Models;
using OfficeOpenXml;

namespace App.Domain.Utils;

public static class MyExcel
{
    public static void WriteSheet(ExcelWorksheet sheet, List<ModifiedOrder> orders)
    {
        sheet.Cells[1, 1].Value = "ПК";
        sheet.Cells[1, 2].Value = "Пользователь";
        sheet.Cells[1, 3].Value = "Дата создания";
        sheet.Cells[1, 4].Value = "Список книг";
        sheet.Cells[1, 5].Value = "Стоимость";

        var i = 2;
        foreach (var order in orders)
        {
            sheet.Cells[i, 1].Value = order.Order.Id;
            sheet.Cells[i, 2].Value = order.Order.User.Email;
            sheet.Cells[i, 3].Value = order.Order.Date;
            sheet.Cells[i, 3].Style.Numberformat.Format = "dd.mm.yyyy";

            var total = new decimal();
            var sb = new StringBuilder();
            foreach (var book in order.Books)
            {
                total += book.InOrderCount * book.Standard.Cost;
                sb.Append($"{book.InOrderCount}x {book.Standard.Name} ({book.Standard.Year})\n");
            }
            sheet.Cells[i, 4].Value = sb.ToString();
            sheet.Cells[i, 5].Value = total;

            i++;
        }

        sheet.Cells[1, 1, orders.Count + 1, 5].AutoFitColumns();
    }

    public static async Task SaveExcelAsync(string path, ExcelPackage package)
    {
        var bytes = await package.GetAsByteArrayAsync();
        try
        {
            await File.WriteAllBytesAsync(path, bytes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}