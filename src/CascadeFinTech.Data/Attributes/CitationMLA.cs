using System;

namespace CascadeFinTech.Data.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class CitationMLA : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
