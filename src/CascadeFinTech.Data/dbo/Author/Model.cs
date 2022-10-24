using CascadeFinTech.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CascadeFinTech.Data.dbo.Author
{
    internal class Model : BaseGuid
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
    }
}
