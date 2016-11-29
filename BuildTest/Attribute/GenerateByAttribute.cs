using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GenerateByAttribute:System.Attribute
    {
        private string _generator;
        public string Generator {
            get
            {
                return _generator;
            }
        }

        private GeneratorType _type;
        public GeneratorType Type
        {
            get
            {
                return _type;
            }
        }

        public GenerateByAttribute(string generatorName,GeneratorType type)
        {
            _generator = generatorName;
            _type = type;
        }
    }
}
