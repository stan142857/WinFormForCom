/************************************************************
* Copyright (c) 2020 All Rights Reserved.
* CLR版本: 4.0.30319.42000
* 机器名称: DESKTOP-9H2IJLA
* 公司名称: 
* 命名空间: mingrisoft_3__test_v._0._0._1
* 文件名: sqlHelper
* 版本号: v1.0.0.0
* 唯一标识: b409d364-5df0-4474-9bcb-ad3a40835e07
* 当前的用户域: $userdomins$
* 创建人: stan
* 电子邮箱: stan142857@outlook.com
* 创建时间: 2020-03-31 19:19:22

* 描述
*
*=================================================================
* 修改标识
* 修改时间: 2020-03-31 19:19:22
* 修改人:   stan
* 版本号: v1.0.0.0
* 描述:
*
****************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;


namespace mingrisoft_3__test_v._0._0._1
{

    public class sqlHelper
    {
         static string sqlConnect = "Data Source=DESKTOP-9H2IJLA\\MSSQLDEVELOP17;User ID=mingrisoft;Password=mingrisoft;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         SqlConnection con = new SqlConnection(sqlConnect);
        SqlDataReader sdr;
        public SqlConnection GetCon()
        {
            return con;
        }
        public SqlDataReader QueryOperationProc(string StrQueryCommand)
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = StrQueryCommand;
            if(sdr != null)
            {
                sdr.Close();
            }
            sdr = cmd.ExecuteReader();
            return sdr;
        }
        public DataTable Query(String StrQueryCommand)
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(StrQueryCommand, con);
            sda.Fill(dt);
            return dt;
        }
        public bool ExeNonQuery(String StrCmd)
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = StrCmd;
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;

        }
        public void closeConn()
        {
            if (con.State != System.Data.ConnectionState.Closed)
                con.Close();
        }

        public bool ExeNoQueryProc(String cmdName, SqlParameter[] ps)
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = cmdName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            foreach (SqlParameter p in ps)
            {
                cmd.Parameters.Add(p);
            }
            try
            {
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public DataTable QueryOperationProc(String cmdName, SqlParameter[] ps)
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = cmdName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if (ps != null)
            {
                foreach (SqlParameter p in ps)
                {
                    cmd.Parameters.Add(p);
                }
            }
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            return dt;
        }

        public DataTable QueryProc(String cmdStr, SqlParameter[] ps)
        {
            DataTable dt = new DataTable();
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = cmdStr;
            if (ps != null)
            {
                foreach (SqlParameter p in ps)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }
    }
}
