using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Services;

public class PagedResult<T> where T : class
{
    public int Pages { get; set; } = 1;

    public required IEnumerable<T> Values { get; set; }
}
