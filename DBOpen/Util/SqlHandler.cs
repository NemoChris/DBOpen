namespace DBOpen.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;    
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    public class SqlHandler
    {
        public static DataSet ExecuteDataSet(string Conn, string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    new SqlDataAdapter(SQLString, connection).Fill(dataSet);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
                return dataSet;
            }
        }

        public static DataSet ExecuteDataSet(string Conn, string SQLString, SqlParameter[] cmdParms)
        {
            DataSet set2;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        adapter.Fill(dataSet);
                        cmd.Parameters.Clear();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }
                    set2 = dataSet;
                }
            }
            return set2;
        }

        public static SqlDataReader ExecuteReader(string Conn, string SQLString)
        {
            SqlDataReader reader;
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]);
            SqlCommand command = new SqlCommand(SQLString, connection);
            try
            {
                connection.Open();
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return reader;
        }

        public static SqlDataReader ExecuteReader(string Conn, string SQLString, SqlParameter[] cmdParms)
        {
            SqlDataReader reader2;
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings[Conn]);
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, conn, null, SQLString, cmdParms);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                reader2 = reader;
            }
            catch (Exception exception)
            {
                cmd.Dispose();
                conn.Close();
                throw new Exception(exception.Message);
            }
            return reader2;
        }

        public static void ExecuteSql(string Conn, Hashtable SQLHashtable)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        foreach (string str in SQLHashtable.Keys)
                        {
                            List<object> list = (List<object>) SQLHashtable[str];
                            string cmdText = (string) list[0];
                            SqlParameter[] cmdParms = (SqlParameter[]) list[1];
                            PrepareCommand(cmd, connection, transaction, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw new Exception(exception.Message);
                    }
                    cmd.Dispose();
                }
            }
        }

        public static int ExecuteSql(string Conn, string SQLString)
        {
            int num2;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                SqlCommand command = new SqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    num2 = command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            return num2;
        }

        public static int ExecuteSql(string Conn, string SQLString, SqlParameter[] cmdParms)
        {
            int num2;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    PrepareCommand(command, connection, null, SQLString, cmdParms);
                    try
                    {
                        int num = command.ExecuteNonQuery();
                        command.Parameters.Clear();
                        num2 = num;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }
                }
            }
            return num2;
        }

        public static int ExecuteSqlScalar(string Conn, string SQLString)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                SqlCommand command = new SqlCommand(SQLString, connection);
                try
                {
                    connection.Open();
                    object objA = command.ExecuteScalar();
                    if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
                    {
                        return -1;
                    }
                    num = int.Parse(objA.ToString());
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
            return num;
        }

        public static int ExecuteSqlScalar(string Conn, string SQLString, SqlParameter[] cmdParms)
        {
            int num;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    PrepareCommand(command, connection, null, SQLString, cmdParms);
                    try
                    {
                        object objA = command.ExecuteScalar();
                        command.Parameters.Clear();
                        if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
                        {
                            return -1;
                        }
                        num = int.Parse(objA.ToString());
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }
                }
            }
            return num;
        }

        public static DataSet ExecuteStoredProc(string Conn, string storedProcName, SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                DataSet dataSet = new DataSet();
                SqlCommand selectCommand = new SqlCommand(storedProcName, connection) {
                    CommandTimeout = 0x5dc,
                    CommandType = CommandType.StoredProcedure
                };
                if (cmdParms != null)
                {
                    foreach (SqlParameter parameter in cmdParms)
                    {
                        if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        selectCommand.Parameters.Add(parameter);
                    }
                }
                try
                {
                    new SqlDataAdapter(selectCommand).Fill(dataSet);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
                return dataSet;
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0x5dc;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}

