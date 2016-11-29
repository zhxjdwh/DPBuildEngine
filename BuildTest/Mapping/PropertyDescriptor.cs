using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildTest.Attribute;

namespace BuildTest.Mapping
{
    /// <summary>
    /// 实体属性描述
    /// </summary>
    public class PropertyDescriptor
    {
        /// <summary>
        /// 根据属性生成PropertyDescriptor,如果是Ignore 属性,则返回null
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static PropertyDescriptor Parse(PropertyInfo prop)
        {
            var name = prop.Name;
            var key = prop.GetCustomAttribute<KeyAttribute>(false);
            var generat = prop.GetCustomAttribute<GenerateByAttribute>(false);
            var ignore = prop.GetCustomAttribute<IgnoreAttribute>(false);
            var columnType = prop.GetCustomAttribute<ColumnTypeAttribute>(false);
            if (ignore != null)
            {
                return null;
            }
            return new PropertyDescriptor(name, generat, key,prop.PropertyType,columnType);
        }

        public PropertyDescriptor(string name, GenerateByAttribute generateBy, KeyAttribute keyAttribute,Type clrType,ColumnTypeAttribute columnType)
        {
            Name = name;
            GenerateBy = generateBy;
            Key = keyAttribute;
            ClrType = clrType;
            ColumnType = columnType;
        }
        public string Name { get; private set; }

        public GenerateByAttribute GenerateBy { get; private set; }

        public KeyAttribute Key { get; private set; }

        public Type ClrType { get;private set; }

        public ColumnTypeAttribute ColumnType { get;private set; }



    }
}
