using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RsaHelper_Go
{
    public static class SqlServerHelper
    {
        /// <summary>  
        /// 执行数据库非查询操作,返回受影响的行数  
        /// </summary>  
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前操作影响的数据行数</returns>  
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>  
        /// 执行数据库事务非查询操作,返回受影响的行数  
        /// </summary>  
        /// <param name="transaction">数据库事务对象</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前事务操作影响的数据行数</returns>  
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>  
        /// 执行数据库非查询操作,返回受影响的行数  
        /// </summary>  
        /// <param name="connection">SqlServer数据库连接对象</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前操作影响的数据行数</returns>  
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (connection == null)
                throw new ArgumentNullException("当前数据库连接不存在");
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>  
        /// 执行数据库查询操作,返回SqlDataReader类型的内存结果集  
        /// </summary>  
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前查询操作返回的SqlDataReader类型的内存结果集</returns>  
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return reader;
            }
            catch
            {
                cmd.Dispose();
                conn.Close();
                throw;
            }
        }

        /// <summary>  
        /// 执行数据库查询操作,返回DataSet类型的结果集  
        /// </summary>  
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前查询操作返回的DataSet类型的结果集</returns>  
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            DataSet ds = null;
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                ds = new DataSet();
                adapter.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            return ds;
        }

        /// <summary>  
        /// 执行数据库查询操作,返回DataTable类型的结果集  
        /// </summary>  
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前查询操作返回的DataTable类型的结果集</returns>  
        public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            DataTable dt = null;

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                dt = new DataTable();
                adapter.Fill(dt);
                cmd.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            return dt;
        }

        /// <summary>  
        /// 执行数据库查询操作,返回结果集中位于第一行第一列的Object类型的值  
        /// </summary>  
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前查询操作返回的结果集中位于第一行第一列的Object类型的值</returns>  
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            object result = null;
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            return result;
        }

        /// <summary>  
        /// 执行数据库事务查询操作,返回结果集中位于第一行第一列的Object类型的值  
        /// </summary>  
        /// <param name="trans">一个已存在的数据库事务对象</param>  
        /// <param name="commandType">命令类型</param>  
        /// <param name="commandText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前事务查询操作返回的结果集中位于第一行第一列的Object类型的值</returns>  
        public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (trans == null)
                throw new ArgumentNullException("当前数据库事务不存在");
            SqlConnection conn = trans.Connection;
            if (conn == null)
                throw new ArgumentException("当前事务所在的数据库连接不存在");

            SqlCommand cmd = new SqlCommand();
            object result = null;

            try
            {
                PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                trans.Dispose();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            return result;
        }

        /// <summary>  
        /// 执行数据库查询操作,返回结果集中位于第一行第一列的Object类型的值  
        /// </summary>  
        /// <param name="conn">数据库连接对象</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        /// <returns>当前查询操作返回的结果集中位于第一行第一列的Object类型的值</returns>  
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            if (conn == null) throw new ArgumentException("当前数据库连接不存在");
            SqlCommand cmd = new SqlCommand();
            object result = null;

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                result = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="connection">SqlServer数据库连接对象</param>  
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader对象</returns>
        public static SqlDataReader RunStoredProcedure(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlDataReader returnReader = null;
            connection.Open();
            SqlCommand command = BuildSqlCommand(connection, storedProcName, parameters);
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;
        }


        /// <summary>  
        /// 执行数据库命令前的准备工作  
        /// </summary>  
        /// <param name="cmd">Command对象</param>  
        /// <param name="conn">数据库连接对象</param>  
        /// <param name="trans">事务对象</param>  
        /// <param name="cmdType">Command类型</param>  
        /// <param name="cmdText">SqlServer存储过程名称或PL/SQL命令</param>  
        /// <param name="cmdParms">命令参数集合</param>  
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// 构建SqlCommand对象
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildSqlCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }
    }
}
