using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CascadeFinTech.Data.dbo
{
    internal interface ITable<T>
    {
        T DataReader(IDataReader reader);
    }
}
