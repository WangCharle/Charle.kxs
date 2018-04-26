using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// WWJ
    /// 20180331
    /// sqlserver的连接池
    /// </summary>
    public class SQLServerPoolHelper
    {
        private readonly static Queue<SqlConnection> conqueuesql = new Queue<SqlConnection>();

        private readonly static object ConnectPoolHelperLocksql = new object();

        private static int conncountsql = 5;


        public static SqlConnection CreateConnect(string connstr)
        {
            if (conqueuesql.Count <= conncountsql)
            {
                lock (ConnectPoolHelperLocksql)
                {
                    if (conqueuesql.Count <= conncountsql)
                    {
                        conqueuesql.Enqueue(new SqlConnection(connstr));
                    }

                }
            }
            lock (ConnectPoolHelperLocksql)
            {
                SqlConnection tempcon = null;
                for (int i = 0; i < conqueuesql.Count; i++)
                {
                    tempcon = conqueuesql.Dequeue();
                    if (tempcon != null)
                    {
                        return tempcon;
                    }
                }
                tempcon = new SqlConnection(connstr);
                conqueuesql.Enqueue(tempcon);
                return tempcon;
            }



        }

        public static void AddMySqlConnection(SqlConnection conn)
        {
            if (conn != null)
            {
                lock (ConnectPoolHelperLocksql)
                {
                    if (conn != null)
                    {
                        if (conqueuesql.Count <= conncountsql)
                        {
                            conqueuesql.Enqueue(conn);
                        }
                        else
                        {
                            conn.Close();
                            conn.Dispose();
                        }

                    }

                }
            }

        }


    }
}
