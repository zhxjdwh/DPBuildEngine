using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildTest.Common
{
    public static class ReflectTypeEnumerator
    {
        private static event Action<string, Type, Assembly> OnTypeFound;
        private static long _isExecuted = 0;

        public static void Register(Action<string, Type, Assembly> action)
        {
            OnTypeFound += action;
        }

        private static void OnOnTypeFound(string arg1, Type arg2, Assembly arg3)
        {
            OnTypeFound?.Invoke(arg1, arg2, arg3);
        }

        public static void Begin()
        {
            if (Interlocked.Increment(ref _isExecuted)>1)
            {
                throw new InvalidOperationException("ReflectTypeEnumerator.Begin() can not be invoke 2+ times!!");
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var exportedType in assembly.GetExportedTypes())
                {
                    OnOnTypeFound(exportedType.Name,exportedType,assembly);
                }
            }
        }
    }
}
