using IneAppWeb.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbeAppWeb.DTOs;
public class ProjectCustomerInvoiceDto : Project
{
    public XCustomer? Customer { get; set; }
    public List<XInvoice> Invoices { get; set; } = new List<XInvoice>();
}
