using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Logic
{
    class DataBase
    {
        private static MySqlConnection mySqlConnection = new MySqlConnection(Config.CONNECT);

        public static DataRow getNewRow(string sql)
        {
            mySqlConnection.Open();

            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mySqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            mySqlConnection.Close();
            return dt.NewRow();
        }

        //public static MySqlConnection getConnection()
        //{
        //    mySqlConnection.Open();
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(
        //        queryString, connection);
        //    adapter.Fill(dataset);
        //    return dataset;

        //}

        //public static void closeConnection()
        //{
        //    mySqlConnection.Close();
        //}

        public static void create(string sql, DataRow row)
        {
            mySqlConnection.Open();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, mySqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            dt.Rows.Add(row.ItemArray);
            new MySqlCommandBuilder(adapter);
            adapter.Update(ds);
            mySqlConnection.Close();
        }

        public static DataRow getRow(string sql)
        {
            MySqlCommand sqlCom = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();

            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);

            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.Rows[0];

            mySqlConnection.Close();
            return row;
        }

        public static DataRowCollection getRows(string sql)
        {
            MySqlCommand sqlCom = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();

            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);

            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            DataRowCollection rows = dt.Rows;
            mySqlConnection.Close();
            return rows;
        }

        public static void query(string sql)
        {
            MySqlCommand comm = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            comm.ExecuteNonQuery();
            mySqlConnection.Close();
        }
    }
}
