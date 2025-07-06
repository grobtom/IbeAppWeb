using IbeAppWeb.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs;

/// <summary>
/// Represents a data transfer object (DTO) for an installment invoice, containing details about the invoice,  its
/// position, associated text, and financial information such as quantity, unit price, and total position sum.
/// </summary>
/// <remarks>This DTO is typically used to transfer data related to installment invoices between application
/// layers.  It includes optional and required fields for invoice details, such as the position description,  quantity,
/// and calculated financial values.</remarks>
public sealed class AbschlagsRechnungDto
{
    public int PkHzlh { get; init; }
    public string? Abschlagsrechnung { get; init; }
    public string? Position { get; init; }
    public string? Kurztext { get; init; }
    public DateTime? SaniertAm { get; init; }
    [JsonConverter(typeof(Decimal3Converter))]
    public decimal? Menge { get; init; }
    public string? Einheit { get; init; }
    [JsonConverter(typeof(Decimal3Converter))]
    public decimal Einheitspreis { get; init; }
    [JsonConverter(typeof(Decimal3Converter))]
    public decimal Positionssumme { get; init; }
}
