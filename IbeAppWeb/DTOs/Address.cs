using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs;

/// <summary>
/// Represents a physical address, including street, house number, postal code, and city.
/// </summary>
/// <remarks>This class is commonly used to store and manage address information for entities such as customers,
/// businesses, or locations.</remarks>

public sealed class Address
{
    public int AddressId { get; set; }
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
