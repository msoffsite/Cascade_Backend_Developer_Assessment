using System.Data;

namespace CascadeFinTech.Data.dbo
{
    internal interface ITable<T>
    {
        T DataReader(IDataReader reader);
    }
}
