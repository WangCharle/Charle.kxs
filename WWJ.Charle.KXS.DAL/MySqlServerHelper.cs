using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// SQlServer的具体类
    /// wwj
    /// 20180330
    /// 
    /// </summary>
    public class MySqlServerHelper : AMyHelper, IMyADOHelper
    {
        public override void EnqueueConn(IDbConnection conn)
        {
            SQLServerPoolHelper.AddMySqlConnection((SqlConnection)conn);
        }

        public override IDbCommand GetCommand()
        {
            try
            {
                return new SqlCommand();
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
                return SQLServerPoolHelper.CreateConnect(ConfigurationManager.ConnectionStrings["sqlconn"].ConnectionString);
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
                return new SqlDataAdapter();
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
                SqlParameter para= new SqlParameter(paraname, paravalue);
                para.DbType = type;//goto:鸡肋需要优化wwj20180331
                return para;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
