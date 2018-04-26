using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using MysqlToInfor.Utility;

namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// 用于继承的通用方法
    /// wwj
    /// 20180223
    /// </summary>
    public abstract class AMyHelper
    {

        #region  事务
        private IDbTransaction _MyCurrentTran = null;

        public IDbTransaction MyCurrentTran
        {
            get { return _MyCurrentTran; }

        }

        /// <summary>
        /// 开启事务
        /// 阅读器与事务都需要打开连接
        /// </summary>
        public void BeginCurrentTran()
        {
            IDbConnection conn = GetConnection();
            if (conn.State==ConnectionState.Closed)
            {
                conn.Open();
            }
            if (conn.State==ConnectionState.Broken)
            {
                conn.Close();
                conn.Open();
            }
            _MyCurrentTran = conn.BeginTransaction();

        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitCurrentTran()
        {
            if (_MyCurrentTran != null)
            {
                _MyCurrentTran.Commit();

            }

        }
        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBackCurrentTran()
        {
            if (_MyCurrentTran != null)
            {
                _MyCurrentTran.Rollback();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void DisposableCurrentTran()
        {
            if (_MyCurrentTran != null)
            {
                if (_MyCurrentTran.Connection != null)
                {
                    //_MyCurrentTran.Connection.Close();

                    //_MyCurrentTran.Connection.Dispose();

                    //回收事务连接wwj
                    EnqueueConn(_MyCurrentTran.Connection);
                }
                _MyCurrentTran.Dispose();
            }
        }
        #endregion




        #region 参数

        private List<IDataParameter> _ParaList;

        /// <summary>
        /// 获取参数
        /// </summary>
        public List<IDataParameter> ParaList
        {
            get { return _ParaList; }

        }


        /// <summary>
        /// 清空参数
        /// </summary>
        public void ClearParas()
        {
            if (_ParaList != null && _ParaList.Count > 0)
            {
                _ParaList.Clear();
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="paraname"></param>
        /// <param name="paravalue"></param>
        public void SetPara(string paraname, object paravalue, DbType type)
        {
            if (_ParaList == null)
            {
                _ParaList = new List<IDataParameter>();
            }

            _ParaList.Add(GetPara(paraname, paravalue, type));


        }

        #endregion



        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection GetConnection();

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        public abstract IDbCommand GetCommand();




        /// <summary>
        /// 获取适配器
        /// </summary>
        /// <returns></returns>
        public abstract IDbDataAdapter GetDataAdapter();





        /// <summary>
        /// 获取实现类参数
        /// </summary>
        /// <param name="paraname"></param>
        /// <param name="paravalue"></param>
        /// <returns></returns>
        public abstract IDataParameter GetPara(string paraname, object paravalue, DbType type = DbType.String);


        /// <summary>
        /// 回收连接
        /// </summary>
        /// <param name="conn"></param>
        public abstract void EnqueueConn(IDbConnection conn);

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, List<IDataParameter> paras = null)
        {
            IDbConnection conn = null;
            try
            {
                DataSet ds = new DataSet();
                conn = GetConnection();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }

                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }
                    IDbDataAdapter da = GetDataAdapter();
                    da.SelectCommand = com;
                    da.Fill(ds);
                }
              

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                // LoggerManager.Instance.Error(ex);
                //日志记录
                throw ex;//return null;
            }
            finally
            {
                if (conn != null)//脏链接废弃，WWJ20180412
                {
                    if ( conn.State == ConnectionState.Open)
                    {
                        EnqueueConn(conn);
                    }
                    else
                    {
                        try
                        {
                            conn.Close();
                        }

                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            conn.Dispose();
                            conn = null;
                        }
                    }
                    

                }

            }



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
            IDbConnection conn = null;

            try
            {


                if (tan != null)
                {
                    conn = tan.Connection;
                }
                else
                {
                    conn = GetConnection();
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }

                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }

                    if (tan != null)
                    {
                        com.Transaction = tan;

                    }
                    return com.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                if (tan != null)
                {
                   // LoggerManager.Instance.Error(ex);
                    throw ex;//当有事务的时候直接抛给外层处理wwj,记得事务中来释放资源
                }

                // LoggerManager.Instance.Error(ex);
                //写记录
                throw ex; //return -1;
            }
            finally
            {
                if (tan == null)
                {
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            EnqueueConn(conn);
                        }
                        else
                        {
                            try
                            {
                                conn.Close();
                            }

                            catch (Exception ex)
                            {


                            }
                            finally
                            {
                                conn.Dispose();
                                conn = null;
                            }
                        }



                    }

                }
            }







        }

        /// <summary>
        /// 获取DataReader
        /// </summary>
        /// <returns></returns>
        public void GetDataReader<T>(string sql,Action<T> actiondr, List<IDataParameter> paras = null) where T : IDataReader
        {
            IDbConnection conn = null;
            IDataReader reader = null;
            try
            {
                conn = GetConnection();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }
                    reader = com.ExecuteReader();//CommandBehavior.CloseConnection这样调用的话,需要手动关闭IDataReader wwj

                    if (actiondr != null)
                    {
                        actiondr((T)reader);//reader与connection的关联在此处理wwj
                    }
                    reader.Close();
                    reader.Dispose();

                }


            }
            catch (Exception ex)
            {
                if (reader != null)
                {

                    reader.Close();
                    reader.Dispose();

                }
                // LoggerManager.Instance.Error(ex);


            }
            finally
            {
                if (conn != null)
                {

                    if (conn.State == ConnectionState.Open)
                    {
                        EnqueueConn(conn);
                    }
                    else
                    {
                        try
                        {
                            conn.Close();
                        }

                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            conn.Dispose();
                            conn = null;
                        }
                    }

                }
            }
        }




        /// <summary>
        /// 获取DataReader
        /// </summary>
        /// <returns></returns>
        public IDataReader GetDataReader(string sql, List<IDataParameter> paras = null)
        {
            IDbConnection conn = null;
            IDataReader reader = null;
            try
            {
                conn = GetConnection();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }
                    reader= com.ExecuteReader(CommandBehavior.CloseConnection);//这样调用的话,需要手动关闭IDataReader wwj

                    return reader;
                }


            }
            catch (Exception ex)
            {
                if (reader!=null)
                {
                    reader.Close();
                    reader.Dispose();

                }
                if (conn != null)
                {

                    conn.Close();
                    conn.Dispose();
                }
                // LoggerManager.Instance.Error(ex);

                throw ex;// return null;
            }
        }

        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <returns></returns>
        public object GetScalar(string sql, List<IDataParameter> paras = null)
        {

            IDbConnection conn = null;
            try
            {
                conn = GetConnection();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }
                    return com.ExecuteScalar();


                }


            }
            catch (Exception ex)
            {

                // LoggerManager.Instance.Error(ex);
                throw ex;// return null; 扔出异常,交给上层判断wwj20180418
            }
            finally
            {
                if (conn != null)
                {

                    if (conn.State == ConnectionState.Open)
                    {
                        EnqueueConn(conn);
                    }
                    else
                    {
                        try
                        {
                            conn.Close();
                        }

                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            conn.Dispose();
                            conn = null;
                        }
                    }

                }

            }



        }



        /// <summary>
        /// 重载来兼容以前的方法
        /// wwj
        /// 20180416
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="tan"></param>
        /// <returns></returns>
        public int MyExecuteNonQuery<T>(string sql, List<T> paras = null, IDbTransaction tan = null) where T : IDataParameter
        {
            IDbConnection conn = null;

            try
            {


                if (tan != null)
                {
                    conn = tan.Connection;
                }
                else
                {
                    conn = GetConnection();
                }
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }

                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }

                    if (tan != null)
                    {
                        com.Transaction = tan;

                    }
                    return com.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                if (tan != null)
                {
                  //  LoggerManager.Instance.Error(ex);
                    throw ex;//当有事务的时候直接抛给外层处理wwj,记得事务中来释放资源
                }

                // LoggerManager.Instance.Error(ex);
                //写记录
                throw ex;// return -1;扔出异常较少上层判断 wwj 20180418
            }
            finally
            {
                if (tan == null)
                {
                    if (conn != null)
                    {

                        if (conn.State == ConnectionState.Open)
                        {
                            EnqueueConn(conn);
                        }
                        else
                        {
                            try
                            {
                                conn.Close();
                            }

                            catch (Exception ex)
                            {


                            }
                            finally
                            {
                                conn.Dispose();
                                conn = null;
                            }
                        }

                    }

                }
            }







        }



        /// <summary>
        /// 获取DataReader
        /// WWJ,由于操作频繁,有必要保留连接20180414
        /// 使用泛型转成具体的阅读器
        /// 返回bool类型,重载,不影响原来调用者wwj20160416
        /// </summary>
        /// <returns></returns>
        public bool GetDataReader<T>(string sql, Func<T, bool> readeraction, List<IDataParameter> paras = null) where T : IDataReader
        {
            IDbConnection conn = null;
            T t = default(T);
            try
            {
                conn = GetConnection();

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (conn.State == ConnectionState.Broken)
                {
                    conn.Close();
                    conn.Open();
                }
                using (IDbCommand com = GetCommand())
                {
                    com.Connection = conn;
                    com.CommandText = sql;
                    com.CommandType = CommandType.Text;

                    if (paras != null && paras.Count > 0)
                    {
                        foreach (var item in paras)
                        {
                            com.Parameters.Add(item);
                        }
                    }
                    t = (T)com.ExecuteReader();
                    if (readeraction != null)
                    {
                        return readeraction(t);
                    }
                    else
                    {
                        return false;
                    }



                }


            }
            catch (Exception ex)
            {


               // LoggerManager.Instance.Error(ex);
                return false;
            }




            finally
            {
                if (t != null)//关闭阅读器wwj
                {
                    t.Close();
                    t.Dispose();

                }

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        EnqueueConn(conn);
                    }
                    else
                    {
                        try
                        {
                            conn.Close();
                        }

                        catch (Exception ex)
                        {


                        }
                        finally
                        {
                            conn.Dispose();
                            conn = null;
                        }
                    }
                }

            }
        }





    }
}
