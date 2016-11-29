using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildTest.Attribute;
using BuildTest.Common;
using BuildTest.Mapping;

namespace BuildTest.Config
{
    /// <summary>
    /// 应用程序启动时需要调用Start方法进行初始化
    /// </summary>
    public class StartUp
    {
        public static void Start()
        {
            ReflectTypeEnumerator.Register(RevoleEntityDescriptor);
            ReflectTypeEnumerator.Begin();
        }

        private static void RevoleEntityDescriptor(string name, Type type, Assembly ass)
        {
            if (type.GetCustomAttribute<TableAttribute>() != null)
            {
                lock (Global.Descriptors)
                {
                    Global.Descriptors.Add(type,EntityDescriptor.Parse(type));
                }
            }
        }
    }
}
