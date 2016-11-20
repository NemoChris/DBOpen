namespace DBOpen.Util
{
    using System;
    using System.Data;

    using MySql.Data;
    using MySql.Data.MySqlClient;
    using System.Configuration;
    using System.Collections;
    using System.Collections.Generic;


    public class MySqlHandler
    {
        public static DataSet ExecuteDataSet(string Conn, string SQLString)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    connection.Open();
                    new MySqlDataAdapter(SQLString, connection).Fill(dataSet);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
                return dataSet;
            }
        }

        public static DataSet ExecuteDataSet(string Conn, string SQLString, MySqlParameter[] cmdParms)
        {
            DataSet set2;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
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

        public static MySqlDataReader ExecuteReader(string Conn, string SQLString)
        {
            MySqlDataReader reader;
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]);
            MySqlCommand command = new MySqlCommand(SQLString, connection);
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

        public static MySqlDataReader ExecuteReader(string Conn, string SQLString, MySqlParameter[] cmdParms)
        {
            MySqlDataReader reader2;
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings[Conn]);
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, conn, null, SQLString, cmdParms);
            try
            {
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        foreach (string str in SQLHashtable.Keys)
                        {
                            List<object> list = (List<object>) SQLHashtable[str];
                            string cmdText = (string) list[0];
                            MySqlParameter[] cmdParms = (MySqlParameter[]) list[1];
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
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                MySqlCommand command = new MySqlCommand(SQLString, connection);
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

        public static int ExecuteSql(string Conn, string SQLString, MySqlParameter[] cmdParms)
        {
            int num2;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                using (MySqlCommand command = new MySqlCommand())
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
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                MySqlCommand command = new MySqlCommand(SQLString, connection);
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

        public static int ExecuteSqlScalar(string Conn, string SQLString, MySqlParameter[] cmdParms)
        {
            int num;
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                using (MySqlCommand command = new MySqlCommand())
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

        public static DataSet ExecuteStoredProc(string Conn, string storedProcName, MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.AppSettings[Conn]))
            {
                DataSet dataSet = new DataSet();
                MySqlCommand selectCommand = new MySqlCommand(storedProcName, connection) {
                    CommandTimeout = 0x5dc,
                    CommandType = CommandType.StoredProcedure
                };
                if (cmdParms != null)
                {
                    foreach (MySqlParameter parameter in cmdParms)
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
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
                return dataSet;
            }
        }

        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText, MySqlParameter[] cmdParms)
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
                foreach (MySqlParameter parameter in cmdParms)
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

