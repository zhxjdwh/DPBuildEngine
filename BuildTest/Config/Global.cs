using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuildTest.Mapping;
using BuildTest.Uow;

namespace BuildTest.Config
{
    internal static class Global
    {
        internal static Dictionary<Type, EntityDescriptor> Descriptors { get; private set; }=new Dictionary<Type, EntityDescriptor>();

    }
}
