using System;
using Microsoft.EntityFrameworkCore;

namespace App.Domain.Models;

public class ReportFilter
{
    public DateTimeOffset Start { get; set; } = new(new DateTime(2025, 3, 27));
    public DateTimeOffset End { get; set; } = DateTime.Now;
}