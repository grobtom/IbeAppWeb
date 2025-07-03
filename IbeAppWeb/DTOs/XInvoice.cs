using System;

namespace IbeAppWeb.DTOs;

/// <summary>
/// Represents an invoice associated with a project, including details such as invoice number, date, amount, and project
/// information.
/// </summary>
/// <remarks>This class is designed to encapsulate invoice-related data for use in project management systems. It
/// includes optional properties for nullable fields, allowing flexibility in scenarios where certain invoice details
/// may be unavailable.</remarks>

public sealed class XInvoice
{
    public int XInvoiceId { get; set; }
    public int? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal? InvoiceAmount { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
}
