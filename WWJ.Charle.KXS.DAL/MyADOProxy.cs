using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// WWJ
    /// 20180228
    /// 代理类操作,扩展接口方法
    /// </summary>
    public class MyADOProxy : IMyADOHelper,IDisposable
    {

        private AMyHelper myhelper = null;

        private static readonly object adoproxylock = new object();//锁对象也是单线程锁


        private static MyADOProxy myadoproxy;

        private MyADOProxy(AMyHelper helper)
        {
            this.myhelper = helper;
        }

        public static MyADOProxy GetInstance(AMyHelper helper)
        {
            if (myadoproxy==null)
            {
                lock (adoproxylock)
                {
                    if (myadoproxy == null)
                    {
                        myadoproxy = new MyADOProxy(helper);
                    }
                }
            }

            return myadoproxy;
        }


        #region 参数部分
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="paraname"></param>
        /// <param name="paravalue"></param>
        /// <param name="type"></param>
        public void SetPara(string paraname, object paravalue, DbType type)
        {
            myhelper.SetPara(paraname, paravalue, type);
        }
        /// <summary>
        /// 清除参数
        /// </summary>
        public void ClearParas()
        {
            myhelper.ClearParas();
        }
        /// <summary>
        /// 获取参数列表
        /// </summary>
        public List<IDataParameter> ParaList
        {
            get
            {
                return myhelper.ParaList;
            }

        }
        #endregion


        #region 操作部分
        /// <summary>
        /// 获取DataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public void GetDataReader<T>(string sql, Action<T> actiondr, List<IDataParameter> paras = null) where T : IDataReader
        {
            myhelper.GetDataReader<T>(sql, actiondr, paras);
        }
        


        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, List<IDataParameter> paras = null)
        {
            return myhelper.GetDataTable(sql, paras);
        }
        /// <summary>
        /// 执行非查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="tan"></param>
        /// <returns></returns>
        public int MyExecuteNonQuery(string sql, List<IDataParameter> paras = null, IDbTransaction tan = null)
        {
            return myhelper.MyExecuteNonQuery(sql, paras, tan);
        }

        public int MyExecuteNonQuery<T>(string sql, List<T> paras = null, IDbTransaction tan = null) where T : IDataParameter
        {
            return myhelper.MyExecuteNonQuery<T>(sql, paras, tan);
        }
        /// <summary>
        /// 获取阅读器对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public IDataReader GetDataReader(string sql, List<IDataParameter> paras = null)
        {
            return myhelper.GetDataReader(sql, paras);
        }

        public bool GetDataReader<T>(string sql, Func<T, bool> readeraction, List<IDataParameter> paras = null) where T : IDataReader
        {
            return myhelper.GetDataReader<T>(sql, readeraction, paras);
        }

        public object GetScalar(string sql, List<IDataParameter> paras = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 底层事务
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginCurrentTran()
        {
             myhelper.BeginCurrentTran();

        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitCurrentTran()
        {
            myhelper.CommitCurrentTran();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBackCurrentTran()
        {
            myhelper.RollBackCurrentTran();
        }

        public void Dispose()
        {
            myhelper.DisposableCurrentTran();
        }

      



        /// <summary>
        /// 获取事务
        /// </summary>
        public IDbTransaction MyCurrentTran
        {
            get
            {
                return myhelper.MyCurrentTran;
            }
        }
        #endregion


    }
}
