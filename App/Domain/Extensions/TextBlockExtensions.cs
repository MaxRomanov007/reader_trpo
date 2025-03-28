using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace App.Domain.Extensions;

public static class TextBlockExtensions
{
    /// <summary>
    /// Устанавливает текст в TextBlock и автоматически очищает его через указанное время.
    /// </summary>
    /// <param name="textBlock">Элемент TextBlock</param>
    /// <param name="text">Текст для отображения</param>
    /// <param name="timeout">Время, через которое текст исчезнет (по умолчанию 3 секунды)</param>
    public static void ShowTemporaryText(this TextBlock textBlock, string text, TimeSpan? timeout = null)
    {
        textBlock.Text = text;

        var timer = new DispatcherTimer
        {
            Interval = timeout ?? TimeSpan.FromSeconds(3)
        };

        timer.Tick += (sender, e) =>
        {
            textBlock.Text = string.Empty;
            timer.Stop();
        };

        timer.Start();
    }
}