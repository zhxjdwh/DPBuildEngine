using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Attribute
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class ColumnTypeAttribute:System.Attribute
    {
        public string ColumnType { get;private set; }

        public ColumnTypeAttribute(string columnType)
        {
            ColumnType = columnType;
        }
    }
}
