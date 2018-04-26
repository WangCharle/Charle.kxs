using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Configuration;
namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// 使用ODBC操作Informix
    /// wwj
    /// 20180224
    /// </summary>
    public class MyODBCHelper : AMyHelper, IMyADOHelper
    {
        public override void EnqueueConn(IDbConnection conn)
        {
            ODBCConnectPoolHelper.AddMySqlConnection((OdbcConnection)conn);
        }

        public override IDbCommand GetCommand()
        {
            try
            {
                return new OdbcCommand();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override IDbConnection GetConnection()
        {
            try
            {
                return ODBCConnectPoolHelper.CreateConnect(ConfigurationManager.ConnectionStrings["InformixConnectionString"].ConnectionString);
                //return new OdbcConnection(ConfigurationManager.ConnectionStrings["InformixConnectionString"].ConnectionString);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override IDbDataAdapter GetDataAdapter()
        {
            try
            {
                return new OdbcDataAdapter();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

     

        public override IDataParameter GetPara(string paraname, object paravalue, DbType type=DbType.String)
        {
            try
            {
                OdbcParameter para= new OdbcParameter(paraname, paravalue);
                para.DbType = type;
                return para;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
