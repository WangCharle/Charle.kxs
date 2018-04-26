using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
namespace WWJ.Charle.KXS.DAL
{
    /* 先排队列的先出队列
     * 
     * 1.把创建的连接放在队列中
     * 池子容量是5
     * 从池子取,用后仍池子里,池子满容量是5
     * */



    /// <summary>
    /// 连接池统一管理
    /// wwj
    /// 20180320
    /// </summary>
    public class MysqlConnectPoolHelper
    {

        private readonly static Queue<MySqlConnection> conqueue = new Queue<MySqlConnection>();

        private readonly static object ConnectPoolHelperLock = new object();

        private static int conncount = 5;

        ///// <summary>
        ///// 连接数
        ///// </summary>
        //public int ConnCount
        //{
        //    set { conncount = value; }
        //}


        public static MySqlConnection CreateConnect(string connstr)
        {
            if (conqueue.Count <= conncount)
            {
                lock (ConnectPoolHelperLock)
                {
                    if (conqueue.Count <= conncount)
                    {
                        conqueue.Enqueue(new MySqlConnection(connstr));
                    }

                }
            }
            lock (ConnectPoolHelperLock)
            {
                MySqlConnection tempcon = null;
                for (int i = 0; i < conqueue.Count; i++)
                {
                    tempcon = conqueue.Dequeue();
                    if (tempcon != null)
                    {
                        return tempcon;
                    }
                }
                tempcon = new MySqlConnection(connstr);
                conqueue.Enqueue(tempcon);
                return tempcon;
            }



        }

        public static void AddMySqlConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                lock (ConnectPoolHelperLock)
                {
                    if (conn != null)
                    {
                        if (conqueue.Count <= conncount)
                        {
                            conqueue.Enqueue(conn);
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
