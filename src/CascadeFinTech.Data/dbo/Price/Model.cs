﻿using System;

namespace CascadeFinTech.Data.dbo.Price
{
    internal class Model : BaseGuid
    {
        internal Guid BookId { get; set; }
        internal Enumeration.Currency Currency { get; set; }
        internal decimal Value { get; set; }
    }
}
