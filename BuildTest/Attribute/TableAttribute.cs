using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Attribute
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class TableAttribute : System.Attribute
    {
        private string _table=string .Empty;

        public string Table
        {
            get { return _table; }
        }

        public TableAttribute()
        {
            var target = this.GetType().GetCustomAttributesData();
        }

        public TableAttribute(string name)
        {
            _table = name;
        }
    }
}
