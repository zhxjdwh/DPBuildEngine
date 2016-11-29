using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BuildTest.Config;
using BuildTest.Database;
using BuildTest.Mapping;

namespace BuildTest.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        private EntityDescriptor _descriptor;
        private DbInstance _instance;

        public BaseRepository()
        {
            _descriptor = Global.Descriptors[typeof(T)];
            _instance = _descriptor.DbInstance.Value;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Add(T entity)
        {
            var cmd = _instance.DbFactory.CreateCommand();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("insert into " + _descriptor.Table);

            var paramFormatter = Expression.Constant(_instance.DatabaseAdapter.DbParameterFormatter);
            var insertSql = Expression.Constant("insert into "+);
        }
    }
}
