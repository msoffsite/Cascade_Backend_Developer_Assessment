using CascadeFinTech.Models;

namespace CascadeFinTech.Data.dbo.Author
{
    internal class Model : BaseGuid
    {
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
    }
}
