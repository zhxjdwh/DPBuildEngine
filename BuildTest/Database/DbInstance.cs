using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Database
{
    /// <summary>
    /// 数据库实例,线程安全
    /// </summary>
    public class DbInstance
    {
        private static object _syncRoot = new object();
        private static ConcurrentDictionary<string, DbInstance> _instances = new ConcurrentDictionary<string, DbInstance>();

        /// <summary>
        /// 添加一个数据库实例
        /// </summary>
        /// <param name="instance"></param>
        public static void Add(DbInstance instance)
        {
            if (instance == null)
            {
                throw new NullReferenceException("instance can not be null");
            }
            if (!_instances.TryAdd(instance.Name, instance))
            {
                throw new Exception($"database instance: {instance.Name} already exists");
            }
        }

        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <param name="dbInstanceName"></param>
        /// <returns></returns>
        public static DbInstance Get(string dbInstanceName)
        {
            DbInstance instance = null;
            if (!_instances.TryGetValue(dbInstanceName, out instance))
            {
                throw new KeyNotFoundException(" can not found the database instance name:"+dbInstanceName);
            }
            return instance;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectString { get; set; }

        /// <summary>
        /// 提供程序
        /// </summary>
        public string DbProvider { get; set; }

        private DbProviderFactory _factory = null;
        /// <summary>
        /// 数据库工厂
        /// </summary>
        public DbProviderFactory DbFactory
        {
            get
            {
                if (_factory == null)
                {
                    lock (_syncRoot)
                    {
                        //再次非空检查,获取锁的过程可能已被创建
                        if (_factory != null)
                        {
                            _factory = DbProviderFactories.GetFactory(this.DbProvider);
                        }
                    }
                }
                return _factory;
            }
        }

        /// <summary>
        /// 数据库适配器
        /// </summary>
        public DatabaseAdapter DatabaseAdapter { get; set; }

        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            var con = DbFactory.CreateConnection();
            return con;
        }


    }
}
