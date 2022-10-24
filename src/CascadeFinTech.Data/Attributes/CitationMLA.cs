using System;
using System.Collections.Generic;
using System.Text;

namespace CascadeFinTech.Data.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class CitationMLA : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
