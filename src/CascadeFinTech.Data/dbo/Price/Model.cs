using CascadeFinTech.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Currency = CascadeFinTech.Data.dbo.Price.Enumeration.Currency;

namespace CascadeFinTech.Data.dbo.Price
{
    internal class Model : BaseGuid
    {
        internal Guid BookId { get; set; }
        internal Currency Currency { get; set; }
        internal decimal Value { get; set; }
    }
}
