using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// ODBC的连接池
    /// wwj
    /// 20180320
    /// </summary>
    public class ODBCConnectPoolHelper
    {
        private readonly static Queue<OdbcConnection> odbcconqueue = new Queue<OdbcConnection>();

        private readonly static object odbcConnectPoolHelperLock = new object();

        private static int odbcconncount = 5;


        public static OdbcConnection CreateConnect(string connstr)
        {
            if (odbcconqueue.Count <= odbcconncount)
            {
                lock (odbcConnectPoolHelperLock)
                {
                    if (odbcconqueue.Count <= odbcconncount)
                    {
                        odbcconqueue.Enqueue(new OdbcConnection(connstr));
                    }

                }
            }
            lock (odbcConnectPoolHelperLock)
            {
                OdbcConnection tempcon = null;
                for (int i = 0; i < odbcconqueue.Count; i++)
                {
                    tempcon = odbcconqueue.Dequeue();
                    if (tempcon != null)
                    {
                        return tempcon;
                    }
                }
                tempcon = new OdbcConnection(connstr);
                odbcconqueue.Enqueue(tempcon);
                return tempcon;
            }



        }

        public static void AddMySqlConnection(OdbcConnection conn)
        {
            if (conn != null)
            {
                lock (odbcConnectPoolHelperLock)
                {
                    if (conn != null)
                    {
                        if (odbcconqueue.Count <= odbcconncount)
                        {
                            odbcconqueue.Enqueue(conn);
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
