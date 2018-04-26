using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace WWJ.Charle.KXS.DAL
{
    /*1.从池中取
     * 2.向池里存
     * */
    /// <summary>
    /// wwj
    /// 20180321
    /// 使用接口约束
    /// </summary>
    public interface ConnectPoolHelper
    {
        /// <summary>
        /// 从连接池获取连接
        /// </summary>
        /// <param name="connstr"></param>
        /// <returns></returns>
        IDbConnection GetConnectFromPool(string connstr);

        /// <summary>
        /// 添加连接到连接池
        /// </summary>
        /// <param name="conn"></param>
        void AddConnectToPool(IDbConnection conn);
      
    }
}
