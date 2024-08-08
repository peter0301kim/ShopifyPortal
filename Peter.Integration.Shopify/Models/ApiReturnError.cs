using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peter.Integration.Shopify.Models;

public class ApiReturnError
{
    public Int64 Time { get; set; }
    public ReturnError Error { get; set; }
}

public class ReturnError
{
    public string Code { get; set; }
    public string Message { get; set; }
}
