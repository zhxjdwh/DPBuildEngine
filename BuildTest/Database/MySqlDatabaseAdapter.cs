using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Database
{
    public class MySqlDatabaseAdapter:DatabaseAdapter
    {
        public override string DbParameterFormatter
        {
            get { return "@{0}"; }
        }
    }
}
