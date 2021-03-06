﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mingrisoft_3__test_v._0._0._1
{
    public partial class Readcsv : Form
    {
        public Readcsv()
        {
            InitializeComponent();
        }

        private void Readcsv_Load(object sender, EventArgs e)
        {
            this.basisTableAdapter.Fill(this.mingrisoftDataSet.Basis);

        }
        private void Button1_Click(object sender, EventArgs e)
        {
            sqlHelper shr = new sqlHelper();
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                //显示
                StreamReader sReader = new StreamReader(textBox1.Text,Encoding.UTF8);
                string strLine = string.Empty;
                while((strLine = sReader.ReadLine()) != null)
                {
                    //MatchCollection col = Regex.Matches(strLine, "(?<=^|,)[^\"]*?(?=,|$)|(?<=^|,\")(?:(\"\")?[^\"]*?)*(?=\",?|$)", RegexOptions.ExplicitCapture);
                    string[] str = strLine.Split(new string[] {","},StringSplitOptions.RemoveEmptyEntries);
                    //显示添加的数据
                    int index3 = this.dataGridView3.Rows.Add();
                    this.dataGridView3.Rows[index3].Cells[0].Value = str[0];
                    this.dataGridView3.Rows[index3].Cells[1].Value = str[1];

                    #region listview 的使用
                    /*
                    listView1.Items[0].Text = str[0];
                    listView1.Items[1].Text = str[1];
                    */

                    #endregion

                    #region 录入数据库
                    SqlParameter[] sp =
                    {
                        new SqlParameter("@Id",SqlDbType.Int),
                        new SqlParameter("description",SqlDbType.NVarChar)
                    };
                    sp[0].Value = str[0];
                    sp[1].Value = str[1];
                    shr.ExeNoQueryProc("Proc_InsertBasis", sp);
                    shr.closeConn();
                    #endregion

                    #region  从数据库绑定datagridview
                    
                    sqlHelper shr1 = new sqlHelper();
                    string sqlSelect = "select * from Basis";
                    SqlDataAdapter sda = new SqlDataAdapter(sqlSelect,shr.GetCon());
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    
                    this.dataGridView1.DataSource = ds.Tables[0];
                    this.dataGridView1.AutoGenerateColumns = false;
                    #endregion
                }
            }
        }

        private void CheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox12.Checked == true)
            {
                checkBox11.Checked = false;
                checkBox11.Enabled = false;
            }else if(checkBox12.Checked == false)
            {
                checkBox11.Enabled = true;
            }
        }

        private void CheckBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked == true)
            {
                checkBox21.Checked = false;
                checkBox21.Enabled = false;
            }
            else if (checkBox22.Checked == false)
            {
                checkBox21.Enabled = true;
            }
        }
        
        //初始化数据库
        private void BtnInit_Click(object sender, EventArgs e)
        {
            sqlHelper shr = new sqlHelper();
            string str = "delete from Basis where Id>0";
            shr.ExeNonQuery(str);
            shr.closeConn();
        }
        #region csv大容量存库
        private void BtnTest_Click(object sender, EventArgs e)
        {
            CSVOpeHelper csv = new CSVOpeHelper();
            Stopwatch SW = new Stopwatch();
            string filPath = @"C:\Users\yuanlei\Desktop\get.csv";
            DataTable dt = CSVOpeHelper.OpenCSV(filPath);
            SW.Start();
            CSVOpeHelper.TableValuedToDB(dt);
            SW.Stop();
        }
        #endregion
    }
}