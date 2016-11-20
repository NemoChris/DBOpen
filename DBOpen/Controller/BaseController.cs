using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;

using DBOpen.Util;

namespace DBOpen.Controller
{
    public abstract class BaseController
    {
        private string _dbName;
        private string DBName
        {
            get
            {
                if (string.IsNullOrEmpty(this._dbName))
                {
                    string typeName = this.GetType().ToString();
                    string dbName = typeName.Substring(typeName.LastIndexOf(".") + 1).Replace("Controller", "");
                    this._dbName = dbName;
                }
                return _dbName;
            }
        }

        public List<T> Query<T>(string sql) where T : new()
        {
            return this.Query<T>(sql, null);
        }

        public List<T> Query<T>(string sql, object[] paramters) where T : new()
        {
            Type type = typeof(T);
            T local = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);

            PropertyInfo[] properties = type.GetProperties();
            string conn = type.GetField("Conn").GetValue(local).ToString();

            DataSet set = this.Query(conn, sql, paramters);

            List<T> list = new List<T>();
            if (set.Tables.Count > 0)
            {
                DataRowCollection rows = set.Tables[0].Rows;
                DataColumnCollection columns = set.Tables[0].Columns;
                if (rows.Count > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        T model = Activator.CreateInstance<T>();

                        foreach (PropertyInfo info in properties)
                        {
                            if (columns.Contains(info.Name))
                            {
                                object propertyValue;
                                if (DBNull.Value != row[info.Name])
                                {
                                    propertyValue = row[info.Name];
                                    ModelProperty<T>.SetValue(model, info.Name, propertyValue);
                                }
                            }
                        }
                        list.Add(model);
                    }
                }
            }
            set.Dispose();
            return list;
        }

        public System.Data.DataSet Query(string conn, string sql)
        {
            return this.Query(conn, sql, null);
        }

        public System.Data.DataSet Query(string conn, string sql, object[] paramters)
        {
            DataSet set = new DataSet();
            string str = "$";

            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType("DBOpen.Util." + DBName + "Handler");

            if (sql.IndexOf(str) < 0)
            {

                Type parameterType;
                parameterType = GetParameterArrTypeByDBName();                
                MethodInfo methodInfo = type.GetMethod("ExecuteDataSet", new Type[] { typeof(string), typeof(string), parameterType });
                return methodInfo.Invoke(null, new object[] { conn, sql, paramters }) as DataSet;
            }

            string storedProcName = sql.Substring(sql.IndexOf(str) + 1);
            MethodInfo methodInfo2 = type.GetMethod("ExecuteStoredProc");
            return methodInfo2.Invoke(null, new object[] { conn, storedProcName, paramters }) as DataSet;
        }

        private Type GetParameterArrTypeByDBName()
        {
            Type parameterType = null;

            if (DBName == "Sql")
            {
                parameterType = typeof(System.Data.SqlClient.SqlParameter[]);
            }
            else if (DBName == "MySql")
            {
                parameterType = typeof(MySql.Data.MySqlClient.MySqlParameter[]);
            }
            return parameterType;
        }

        public int NonQuery(string conn, string sql)
        {
            return this.NonQuery(conn, sql, null);
        }

        public int NonQuery(string conn, string sql, object[] paramters)
        {
            int affectCount;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType("DBOpen.Util." + DBName + "Handler");
            Type parameterType;
            parameterType = GetParameterArrTypeByDBName();
                     
            MethodInfo methodInfo = type.GetMethod("ExecuteSql", new Type[] { typeof(string), typeof(string), parameterType });

            affectCount = Convert.ToInt32(methodInfo.Invoke(null, new object[] { conn, sql, paramters }));
            return affectCount;

        }
    }
}
