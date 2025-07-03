using IbeAppWeb.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IneAppWeb.DTOs;

/// <summary>
/// Represents a consumer entity with personal and contact information,  as well as optional company and address
/// details.
/// </summary>
/// <remarks>This class is typically used to store and manage information about an individual or a company 
/// consumer, including their contact details, company affiliation, and associated address.</remarks>

public class XCustomer
{
    public int CustomerId { get; set; }
    public int? CustomerXId { get; set; } = 0;
    public bool CompanyPrivate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Telephone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? CompanyName { get; set; }
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
}
