using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Threading;
using System.Collections;
using MySql.Data.MySqlClient;

namespace importformaccesstomysql
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string connt_access = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\uaspectr\\Documents\\GitHub\\testing\\DTBSFORTEST.accdb"; // Первая часть строки соединения с базой данных, содержащая версию программы
            string connt_mysql = "Server=localhost; Port=3306; user=root; password=22029311q; Database=dtbsfortest;";

            string qry_to_access = "Select * from "+textBox1.Text.ToString();
            string qry_to_mysql_create_table = "SELECT COLUMN_NAME,COLUMN_TYPE, COLUMN_KEY, EXTRA FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = '"+textBox1.Text.ToString() +"' ORDER BY ORDINAL_POSITION";
            string qry_to_mysql = "";

            
            OleDbDataAdapter daforaccess = new OleDbDataAdapter(qry_to_access,connt_access);
            MySqlConnection conn_for_my_sql = new MySqlConnection(connt_mysql);
            conn_for_my_sql.Open();
            MessageBox.Show("Соединение открыто");
            MySqlCommand sqlcom = new MySqlCommand(qry_to_mysql_create_table, conn_for_my_sql);
            sqlcom.ExecuteNonQuery();

            DataSet ds_for_access = new DataSet();

            daforaccess.Fill(ds_for_access);

            DataSet ds_for_mysql = new DataSet();

            MySqlDataAdapter daformysql = new MySqlDataAdapter(sqlcom);

            daformysql.Fill(ds_for_mysql);

            int count = 0;
            ArrayList arr_for_fields_name = new ArrayList();
            ArrayList arr_for_fields_type = new ArrayList();
            ArrayList arr_for_fields_key = new ArrayList();
            ArrayList arr_for_fields_extra = new ArrayList();

            while (count != Convert.ToInt32(ds_for_mysql.Tables[0].Rows.Count.ToString()))
            {
                arr_for_fields_name.Add(ds_for_mysql.Tables[0].Rows[count][0].ToString());
                arr_for_fields_type.Add(ds_for_mysql.Tables[0].Rows[count][1].ToString());
                arr_for_fields_key.Add(ds_for_mysql.Tables[0].Rows[count][2].ToString());
                arr_for_fields_extra.Add(ds_for_mysql.Tables[0].Rows[count][3].ToString());

                count++;
            }
            count = 0;

            bool flag = true;

            string qry_for_create = "";
            string key = "";

            qry_for_create +=  "CREATE TABLE " + textBox1.Text.ToString() + "( ";

            while (count != Convert.ToInt32(ds_for_mysql.Tables[0].Rows.Count.ToString()))
            {            
               qry_for_create += arr_for_fields_name[count].ToString()+" ";
               qry_for_create += arr_for_fields_type[count].ToString()+" ";

               if (arr_for_fields_key[count].ToString() == "PRI")
               {
                   key = "PRIMARY KEY";
                   qry_for_create += key + " ";
               }
               else
               {
                   qry_for_create += arr_for_fields_key[count].ToString() + " ";
               }

               qry_for_create += arr_for_fields_extra[count].ToString() + " ";

                if (count != Convert.ToInt32(ds_for_mysql.Tables[0].Rows.Count.ToString()) - 1)
                {
                    qry_for_create += ",";
                }

                count++;
            }

            qry_for_create += ")";

            //MySqlCommand msqlcmd = new MySqlCommand(qry_for_create, conn_for_my_sql);

            //msqlcmd.ExecuteNonQuery();

            conn_for_my_sql.Close();
            MessageBox.Show("Закрыто");

        }
    }
}
