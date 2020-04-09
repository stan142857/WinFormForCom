/************************************************************
* Copyright (c) 2020 All Rights Reserved.
* CLR版本: 4.0.30319.42000
* 机器名称: DESKTOP-9H2IJLA
* 公司名称: 
* 命名空间: mingrisoft_3__test_v._0._0._1
* 文件名: CSVOpeHelper
* 版本号: v1.0.0.0
* 唯一标识: af9354a4-88c9-48ab-9a70-b3cfe0f16eee
* 当前的用户域: $userdomins$
* 创建人: stan
* 电子邮箱: stan142857@outlook.com
* 创建时间: 2020-04-09 21:33:29

* 描述
*
*=================================================================
* 修改标识
* 修改时间: 2020-04-09 21:33:29
* 修改人:   stan
* 版本号: v1.0.0.0
* 描述:
*
****************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mingrisoft_3__test_v._0._0._1
{
    public class CSVOpeHelper
    {
        public static void TableValuedToDB(DataTable dt)
        {
            sqlHelper shr = new sqlHelper();
            const string TSqlStatement =
             "insert into Basis (Id,description)" +
             " SELECT nc.Id,nc.description" +
             " FROM @NewBulkTestTvp AS nc";
            SqlCommand cmd = new SqlCommand(TSqlStatement, shr.GetCon());
            SqlParameter catParam = cmd.Parameters.AddWithValue("@NewBulkTestTvp", dt);
            catParam.SqlDbType = SqlDbType.Structured;
            //表值参数的名字叫BulkUdt，在上面的建立测试环境的SQL中有。  
            catParam.TypeName = "Basis";
            try
            {
                shr.GetCon().Open();
                if (dt != null && dt.Rows.Count != 0)
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                shr.closeConn();
            }
        }
        public static DataTable OpenCSV(string filePath)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8"); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(strLine))
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
