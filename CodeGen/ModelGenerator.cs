using Microsoft.Build.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using Oracle.DataAccess.Client;


namespace Dragonpass.BuildEngine
{
    public class ModelGenerator : Microsoft.Build.Utilities.Task
    {
        private static ConcurrentDictionary<string, ConcurrentBag<string>> _tables = new ConcurrentDictionary<string, ConcurrentBag<string>>();

        //获取表和字段的注释
        private static string _commentSql = "SELECT ut.table_name CNAME,utc.comments,'1' AS CTYPE,ut.table_name TNAME,NULL AS DTYPE \n"
                                            +
                                            "  FROM user_tables ut LEFT JOIN user_tab_comments utc ON ut.table_name=utc.table_name  \n"
                                            + "WHERE upper(ut.table_name) IN  ({0}) \n"
                                            + "UNION \n"
                                            +
                                            "SELECT utc2.column_name,ucc.comments,'0' AS CTYPE,utc2.table_name TNAME ,utc2.data_type AS DTYPE \n"
                                            +
                                            "  FROM user_tab_columns utc2 LEFT JOIN user_col_comments ucc ON ucc.table_name=utc2.table_name AND utc2.column_name=ucc.column_name \n"
                                            + "WHERE upper(utc2.table_name) IN ({0}) UNION"
                                            +
                                            " SELECT uv.view_name CNAME,'视图'|| uv.view_name comments,'2' AS CTYPE,uv.view_name TNAME,NULL AS DTYPE "
                                            + " FROM user_views uv WHERE upper(uv.view_name) IN({0})  ";


        /// <summary>
        /// 0:属性类型
        /// 1:字段名称
        /// 2:属性名称
        /// 3:双引号
        /// 4:注释说明
        /// </summary>
        private static string _propTemplate =
      @"
        /// <summary>
        /// {<注释说明>}
        /// </summary>
        public {<属性类型>} {<属性名称>} { set; get; }
        ";
        /// <summary>
        /// 0:类名
        /// 1:注释说明
        /// 2:所有属性
        /// 3.双引号
        /// </summary>
        private static string _classTemplate =
        @"
    /// <summary>
    /// {<注释说明>}
    /// </summary>
    [Sequence({<双引号>}SEQ_{<类名>}{<双引号>}), Table({<双引号>}{<类名>}{<双引号>})]
    public class {<类名>}
    {
        #region auto generate property
{<所有属性>}
        #endregion 
    }";


        [Required]
        public ITaskItem[] Files { get; set; }
        [Required]
        public ITaskItem ConnectString { get; set; }

        public ITaskItem OutPut { get; set; }



        public override bool Execute()
        {

            try
            {
                var conStr = ConnectString.ItemSpec;
                if (string.IsNullOrWhiteSpace(conStr))
                {
                    Log.LogWarning("自动生成实体:连接字符串不正确,请重新编辑csproj文件中的UsingTask的ConnectString属性");
                    return true;
                }
                //获取所有tables
                var tabs = _tables.GetOrAdd(conStr, (cstr) =>
                {
                    var t = new ConcurrentBag<string>();
                    using (OracleConnection con = new OracleConnection(conStr))
                    {
                        con.Open();
                        using (
                            OracleCommand cmd =
                                new OracleCommand(
                                    "SELECT t.TABLE_NAME,'1' as TTYPE FROM user_tables t UNION SELECT uv.view_name TABLE_NAME,'2' as TTYPE  FROM user_views uv",
                                    con))
                        {
                            var reader = cmd.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var tname = reader.GetString(0);
                                    var type = reader.GetString(1);
                                    if (!string.IsNullOrWhiteSpace(tname))
                                    {
                                        t.Add(tname.ToUpper());
                                    }
                                }
                            }


                            reader.Close();

                            cmd.CommandText = "";
                        }
                    }
                    return t;
                });

                var tabList = tabs.ToArray().ToList();
                //文件和模板映射  key:文件名,value:表名+模板
                Dictionary<string, string[]> entityTextTpl = new Dictionary<string, string[]>();

                //文件名重复的
                List<string> sameNameFile = new List<string>();

                //遍历所有cs代码文件
                foreach (var item in Files)
                {
                    var fileName = item.ItemSpec;
                    var fileNameWithoutExtension =
                        Path.GetFileNameWithoutExtension(fileName)?.ToUpper(CultureInfo.CurrentCulture);

                    //如果数据库包含与文件同名的表,则继续
                    var matchTableName = tabList.FirstOrDefault(s =>
                    {
                        return fileNameWithoutExtension == s;
                    });

                    if (string.IsNullOrWhiteSpace(matchTableName))
                    {
                        continue;
                    }

                    //如果文件是一个空的类文件,则继续
                    var text = File.ReadAllText(fileName, Encoding.UTF8);
                    var textParses = text.Split('{', '}');
                    if (textParses.Length != 5)
                    {
                        continue;
                    }
                    //将命名空间的内容清空
                    var nameSpaceStart = text.IndexOf("{", StringComparison.Ordinal);
                    var nameSpaceEnd = text.LastIndexOf("}", StringComparison.Ordinal);
                    if (nameSpaceEnd == nameSpaceStart || nameSpaceEnd < 0 || nameSpaceStart < 0)
                    {
                        continue;
                    }
                    if (entityTextTpl.Values.FirstOrDefault(w => w[0] == matchTableName) != null)
                    {
                        sameNameFile.Add(fileName);
                        continue;
                    }
                    text = text.Remove(nameSpaceStart,nameSpaceEnd-nameSpaceStart+1)
                        .Insert(nameSpaceStart, "{@CONTENT@\r\n}");
                    entityTextTpl.Add(fileName, new string[]
                    {
                        matchTableName, text /*,ttype.ToString()*/
                    });
                }
                //去掉文件名重复的
                sameNameFile.ForEach(w =>
                {
                    if (entityTextTpl.ContainsKey(w))
                    {
                        Log.LogWarning($"自动生成实体: 存在同名文件:{w},无法确定代码生成位置,表(视图):{entityTextTpl[w][0]}");
                        entityTextTpl.Remove(w);
                    }
                });

                if (entityTextTpl.Count <= 0)
                {
                    return true;
                }

                //获取表注释 列注释
                List<DbComment> comments = new List<DbComment>();

                using (OracleConnection con = new OracleConnection(conStr))
                {
                    var sql = string.Format(_commentSql,
                        "'" + string.Join("','", entityTextTpl.Values.Select(w => w[0])) + "'");
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(sql, con))
                    {
                        //表
                        var reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                comments.Add(new DbComment()
                                {
                                    CName = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    Comment = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    CType = reader.IsDBNull(2) ? -1 : Convert.ToInt32(reader["CType"]),
                                    TName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    DType = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    //ClrType = null
                                });
                            }
                        }
                        reader.Close();

                    }
                }

                //生成代码文件
                foreach (var fileName in entityTextTpl.Keys)
                {
                    try
                    {
                        var table = entityTextTpl[fileName][0];
                        var tpl = entityTextTpl[fileName][1];
                        var tabCms = comments.FirstOrDefault(w => w.CType == 1 && w.TName == table);
                        var colComss = comments.Where(w => w.TName == table && w.CType == 0);
                        var propCode = string.Join(
                            "",
                            colComss.Select(w =>
                            {
                                ///// 0:属性类型
                                ///// 1:字段名称
                                ///// 2:属性名称
                                ///// 3:双引号
                                ///// 4:注释说明
                                return _propTemplate
                                    .Replace("{<属性类型>}", MapClrTypeString(w?.DType))
                                    .Replace("{<字段名称>}", "_" + w?.CName?.ToLower(CultureInfo.CurrentCulture))
                                    .Replace("{<属性名称>}", w?.CName)
                                    .Replace("{<双引号>}", "\"")
                                    .Replace("{<注释说明>}", w?.Comment?.Replace("\r", "")?.Replace("\n", ""));
                            }
                                ));

                        // 0:类名
                        //1:注释说明
                        // 2:所有属性
                        // 3.双引号
                        var classContent = _classTemplate
                            .Replace("{<类名>}", table)
                            .Replace("{<注释说明>}", tabCms?.CName)
                            .Replace("{<所有属性>}", propCode)
                            .Replace("{<双引号>}", "\"");
                        var code = tpl.Replace("@CONTENT@", classContent);

                        File.WriteAllText(fileName, code);
                        Log.LogWarning("DragonPass Built Engine", "", "", fileName, 0, 0, 0, 0, "成功生成实体:" + table);
                    }
                    catch (Exception ex)
                    {
                        Log.LogWarning("生成实体,错误:" + ex.Message + ex.StackTrace);
                        continue;
                    }

                }

                return true;
            }
            catch (Exception e)
            {
                Log.LogWarning("编译尝试自动生成实体是出错,错误:" + e.Message + e.StackTrace);
            }
            
            return false;
        }

        public string MapClrTypeString(string dbType)
        {
            if (string.IsNullOrEmpty(dbType))
            {
                return "";
            }
            dbType = dbType.ToUpper().Trim();

            switch (dbType)
            {
                case "VARCHAR":
                case "VARCHAR2":
                case "NVARCHAR":
                case "NVARCHAR2":
                case "CHAR":
                case "NCHAR":
                case "CLOB":
                case "NCLOB":
                    return "string";
                case "LONG":
                    return "long";
                case "RAW":
                case "LONG RAW":
                case "BFILE":
                    return "byte[]";
                case "NUMBER":
                    return "decimal";
                case "DECIMAL":
                    return "decimal";
                case "INTEGER":
                    return "int";
                case "FLOAT":
                    return "float";
                case "REAL":
                    return "decimal";
                case "DATE":
                case "DATETIME":
                    return "DateTime";
                default:
                    return "byte[]";
            }
        }


    }

    internal class DbComment
    {
        /// <summary>
        /// 对象名
        /// </summary>
        public string CName { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 类型(1:表,0:字段,2.视图)
        /// </summary>
        public int CType { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DType { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TName { get; set; }
    }
}
