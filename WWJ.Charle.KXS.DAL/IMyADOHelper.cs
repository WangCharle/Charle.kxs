using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// ADO接口
    /// WWJ
    /// 20180223
    /// 后续使用时,用接口扩充功能,IMyADOHelper为最子类的接口
    /// </summary>
    public  interface IMyADOHelper
    {
        /// <summary>
        /// 获取dataTable
        /// </summary>
        DataTable GetDataTable(string sql,List<IDataParameter> paras=null);

        /// <summary>
        /// 执行非查询
        /// </summary>
        /// <returns></returns>
        int MyExecuteNonQuery(string sql, List<IDataParameter> paras=null, IDbTransaction tan=null);

        int MyExecuteNonQuery<T>(string sql, List<T> paras = null, IDbTransaction tan = null) where T : IDataParameter;
        /// <summary>
        /// 获取DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        void GetDataReader<T>(string sql, Action<T> actiondr, List<IDataParameter> paras = null) where T : IDataReader;


        IDataReader GetDataReader(string sql, List<IDataParameter> paras = null);

        bool GetDataReader<T>(string sql, Func<T, bool> readeraction, List<IDataParameter> paras = null) where T : IDataReader;


        object GetScalar(string sql, List<IDataParameter> paras = null);

    }
}
