using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// mysql的ado操作
    /// wwj
    /// 20180223
    /// </summary>
    public class MySQLHelper : AMyHelper, IMyADOHelper
    {
        public override void EnqueueConn(IDbConnection conn)
        {
            MysqlConnectPoolHelper.AddMySqlConnection((MySqlConnection)conn);
        }

        public override IDbCommand GetCommand()
        {
            try
            {
                return new MySqlCommand();
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
                return MysqlConnectPoolHelper.CreateConnect(ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString);


                // return new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString);
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
                return new MySqlDataAdapter();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

   

        public override IDataParameter GetPara(string paraname, object paravalue, DbType type = DbType.String)
        {
            try
            {
                MySqlParameter para= new MySqlParameter(paraname, paravalue);
                para.DbType= type;
                return para;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }
}
