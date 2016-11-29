using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InstanceAttribute : System.Attribute
    {
        public string DbInstance { get; private set; }

        public InstanceAttribute(string dbInstance)
        {
            DbInstance = dbInstance;
        }
    }
}
