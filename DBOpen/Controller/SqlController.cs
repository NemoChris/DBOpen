using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DBOpen.Util;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DBOpen.Controller
{    
    /// <summary>
    /// Object to operate the SqlServer database
    /// </summary>
    public class SqlController : BaseController, IController 
    {
        /// <summary>
        /// Model information for the operation of the database
        /// </summary>
        class ModelInfo
        {
            /// <summary>
            /// The type of model
            /// </summary>
            public Type ObjType { get; set; }

            /// <summary>
            /// Database connection string
            /// </summary>
            public string Conn { get; set; }

            /// <summary>
            /// Database table name
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// primary key name
            /// </summary>
            public string IDFieldName { get; set; }
        }

        public bool Insert<T>(T model) where T : class
        {
            ModelInfo mi = GetModelInfo<T>();
            string format = "insert {0} ({1}) values({2});";
            string clounms = "";
            string values = "";
            List<SqlParameter> list = new List<SqlParameter>();

            foreach (PropertyInfo info in mi.ObjType.GetProperties())
            {
                string name = info.Name;
                object obj2 = ModelProperty<T>.GetValue(model as T, name);
                if (name != mi.IDFieldName)
                {
                    if (clounms.Length > 0)
                    {
                        clounms = clounms + "," + name;
                        values = values + ",@" + name;
                    }
                    else
                    {
                        clounms = clounms + name;
                        values = values + "@" + name;
                    }
                    list.Add(new SqlParameter("@" + name, obj2));
                }
            }

            format = string.Format(format, mi.TableName, clounms, values) + "select SCOPE_IDENTITY();";            
            int propertyValue = SqlHandler.ExecuteSqlScalar(mi.Conn, format, list.ToArray());

            if (propertyValue <= 0)
            {
                return false;
            }
            
            string str5 = mi.ObjType.GetProperty(mi.IDFieldName).PropertyType.ToString();

            if (str5 == null)
            {
                return false;
            }

            if (str5 == "System.Int32")
            {
                ModelProperty<T>.SetValue(model as T, mi.IDFieldName, propertyValue);
            }
            else if (str5 == "System.Int16")
            {
                ModelProperty<T>.SetValue(model as T, mi.IDFieldName, (short)propertyValue);
            }
            else
            {
                ModelProperty<T>.SetValue(model as T, mi.IDFieldName, (long)propertyValue);
            }

            return true;

        }

        private static ModelInfo GetModelInfo<T>()
        {
            ModelInfo mi = new ModelInfo();
            mi.ObjType = typeof(T);
            mi.Conn = mi.ObjType.GetField("Conn").GetValue(null).ToString();
            mi.TableName = mi.ObjType.GetField("TableName").GetValue(null).ToString();
            mi.IDFieldName = mi.ObjType.GetField("IDFieldName").GetValue(null).ToString();
            return mi;
        }


        public bool Delete<T>(T model) where T : class
        {
            ModelInfo mi = GetModelInfo<T>();
            string format = "delete {0} where {1};";
            List<SqlParameter> list = new List<SqlParameter>();
            object fieldValue = ModelProperty<T>.GetValue(model as T, mi.IDFieldName);

            CheckIDField<T>(model, mi, fieldValue);

            format = string.Format(format, mi.TableName, mi.IDFieldName + "=@" + mi.IDFieldName);
            list.Add(new SqlParameter("@" + mi.IDFieldName, fieldValue));

            if (SqlHandler.ExecuteSql(mi.Conn, format, list.ToArray()) <= 0)
            {
                return false;
            }
            return true;
        }

        public bool Delete<T>(string IDListsWillDel) where T : class
        {
            if (string.IsNullOrEmpty(IDListsWillDel))
            {
                throw new Exception("Parameter idListsWillDel is null or empty!");
            }

            if (!new Regex(@"^[0-9,]+$").IsMatch(IDListsWillDel.Trim()))
            {
                throw new Exception("Parameter idListsWillDel is illegal!");
            }

            ModelInfo mi = GetModelInfo<T>();

            string sqlFormat = "delete from {0} where {1} in ({2})";
            sqlFormat = string.Format(sqlFormat, mi.TableName, mi.IDFieldName, IDListsWillDel);
            int affectCount = SqlHandler.ExecuteSql(mi.Conn, sqlFormat);
            return affectCount > 0;
        }

        public bool Update<T>(T model) where T : class
        {
            ModelInfo mi = GetModelInfo<T>();
            string format = "update {0} set {1} where {2};";
            string colunms = "";
            string condition = "";
            List<SqlParameter> list = new List<SqlParameter>();

            foreach (PropertyInfo info in mi.ObjType.GetProperties())
            {
                string name = info.Name;
                object fieldValue = ModelProperty<T>.GetValue(model as T, name);
                list.Add(new SqlParameter("@" + name, fieldValue));

                if (name == mi.IDFieldName)
                {
                    CheckIDField<T>(model, mi, fieldValue);
                    condition = mi.IDFieldName + "=@" + name;
                }
                else if (colunms.Length > 0)
                {
                    string str5 = colunms;
                    colunms = str5 + "," + name + "=@" + name;
                }
                else
                {
                    colunms = colunms + name + "=@" + name;
                }
            }
            format = string.Format(format, mi.TableName, colunms, condition);
            if (SqlHandler.ExecuteSql(mi.Conn, format, list.ToArray()) <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验IDFieldName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="mi"></param>
        /// <param name="fieldValue"></param>
        private static void CheckIDField<T>(T model, ModelInfo mi, object fieldValue) where T : class
        {
            if (fieldValue.Equals(0) || fieldValue.Equals(null))
            {
                throw new Exception(model.ToString() + "’s Identification field" + mi.IDFieldName + "is not assigned");
            }
        }

        public bool Fill<T>(T model) where T : class
        {
            ModelInfo mi = GetModelInfo<T>();

            string format = "select * from {0} where {1}";
            object fieldValue = ModelProperty<T>.GetValue(model as T, mi.IDFieldName);

            CheckIDField<T>(model, mi, fieldValue);

            List<SqlParameter> list = new List<SqlParameter> {
                new SqlParameter("@" + mi.IDFieldName, fieldValue)
            };
            format = string.Format(format, mi.TableName, mi.IDFieldName + "=@" + mi.IDFieldName);

            using (SqlDataReader reader = SqlHandler.ExecuteReader(mi.Conn, format, list.ToArray()))
            {
                while (reader.Read())
                {
                    foreach (PropertyInfo info in model.GetType().GetProperties())
                    {
                        object propertyValue;
                        if (DBNull.Value != reader[info.Name])
                        {
                            propertyValue = reader[info.Name];
                            ModelProperty<T>.SetValue(model as T, info.Name, propertyValue);
                        }
                        // CLR has already given the default value when it is an instance of the object, and does not have to be displayed to the default value.
                        //else // Gets the default value for the type
                        //{
                        //    propertyValue = ReflectionHelper.GetDefaultValueByType(Type.GetType(info.PropertyType.FullName, true));
                        //}                     
                    }

                    return true; // Only return the first data
                }
            }

            return false;
        }

        public bool Save<T>(T model) where T : class
        {
            ModelInfo mi = GetModelInfo<T>();

            object obj2 = ModelProperty<T>.GetValue(model as T, mi.IDFieldName);
            if (!obj2.Equals(0) && !obj2.Equals(null))
            {
                return this.Update(model);
            }
            return this.Insert(model);
        }
    }
}
