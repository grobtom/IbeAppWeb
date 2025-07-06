using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace IbeAppWeb.Abstractions;

/// <summary>
/// Converts <see cref="decimal"/> values to and from JSON, ensuring that values are rounded to three decimal places
/// when written.
/// </summary>
/// <remarks>This converter reads <see cref="decimal"/> values directly from JSON and writes them back as numbers
/// rounded to three decimal places. It is useful for scenarios where consistent precision is required in JSON
/// serialization.</remarks>
public class Decimal3Converter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDecimal();

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        => writer.WriteNumberValue(Math.Round(value, 3));
}
