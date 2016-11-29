using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BuildTest.Attribute;
using BuildTest.Common;
using BuildTest.Database;

namespace BuildTest.Mapping
{
    public class EntityDescriptor
    {
        public static EntityDescriptor Parse(Type entityType)
        {
            var name = entityType.Name;
            var type = entityType;
            var table = entityType.GetCustomAttribute<TableAttribute>(false);
            var instance = entityType.GetCustomAttribute<InstanceAttribute>(false);
            var dic = new Dictionary<string, PropertyDescriptor>();
            foreach (PropertyInfo prop in entityType.GetProperties())
            {
                var pdescirptor = PropertyDescriptor.Parse(prop);
                if (pdescirptor != null)
                {
                    dic.Add(pdescirptor.Name, pdescirptor);
                }
            }
            return new EntityDescriptor(name, type, table, instance, dic);
        }

        public EntityDescriptor(string name, Type entityType, TableAttribute table, InstanceAttribute instance, IEnumerable<KeyValuePair<string, PropertyDescriptor>> props)
        {
            Name = name;
            Type = entityType;
            if (table == null)
            {
                Table = new TableAttribute(name);
            }
            else
            {
                Table = table;
            }
            Instance = instance;
            Propertys = new ReadOnlyConcurrentDictionary<string, PropertyDescriptor>(props);

            DbInstance= new Lazy<DbInstance>(() =>
            {
                return Database.DbInstance.Get(Instance.DbInstance);
            });
        }

        public string Name { get; private set; }

        public Type Type { get; private set; }

        public TableAttribute Table { get; private set; }

        public InstanceAttribute Instance { get; private set; }

        public ReadOnlyConcurrentDictionary<string, PropertyDescriptor> Propertys { get; private set; }

        public Lazy<DbInstance> DbInstance { get;private set; }
    }
}
