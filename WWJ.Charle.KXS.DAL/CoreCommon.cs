//using BPPC.RMS.DataSync.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WWJ.Charle.Model;
//using MysqlToInfor.Utility;

namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// 20180416
    /// 存业务层通用的方法
    /// wwj
    /// </summary>
    public class CoreCommon
    {
        /// <summary>
        /// 获取更新sql
        /// </summary>
        /// <returns></returns>
        public static string GetUpdateSQL<T, P>(T t, string sqlpre, string tbname = "", string pkname = "", List<P> paralist = null, string sqlwhere = "", object pkvalue = null) where T : BaseEntity where P : IDataParameter, new()
        {
            //如果不是从特性中获取主键,表名,必须手动给值
            string sqlstr = string.Empty;

            //sqlpre 后续做成枚举强类型,暂时只考虑Mysql的@

            //P 对于参数化的SQL,采用引用类型传引用处理

            if (t == null || t.ChangeDic == null || t.ChangeDic.Count <= 0)
            {
                return "";
            }
            sqlstr += string.Format(@"update {0} set ", tbname);
            foreach (KeyValuePair<string, object> item in t.ChangeDic)
            {
                sqlstr += item.Key + "=" + sqlpre + item.Key + ",";

                if (paralist != null)
                {

                    paralist.Add(new P() { ParameterName = item.Key, Value = item.Value == null ? DBNull.Value : item.Value });//DBNull.Value转换
                }

            }
            sqlstr = sqlstr.Trim(',');

            if (!string.IsNullOrEmpty(pkname))
            {

                if (paralist != null)
                {
                    if (!t.ChangeDic.ContainsKey(pkname))
                    {
                        t.ChangeDic.Add(pkname, pkvalue);//显示指定主键key,value wwj20180417
                        paralist.Add(new P() { ParameterName = pkname, Value = t.ChangeDic[pkname] == null ? DBNull.Value : t.ChangeDic[pkname] });
                    }

                    if (t.ChangeDic[pkname] != null)
                    {
                        sqlstr += string.Format(" where {0}={1}{0} ", pkname, sqlpre);


                    }
                    else
                    {
                        return "";    //主键值不能为NULL WWJ
                    }



                }
            }
            //else if(true)//从特性中找wwj
            //{

            //}
            else//直接给where 条件 
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                {
                    sqlstr += string.Format(" where {0} ", sqlwhere);
                }
            }






            return sqlstr;

        }


        /// <summary>
        /// 获取插入sql
        /// </summary>
        /// <returns></returns>
        public static string GetInsertSQL<T, P>(T t, string sqlpre, string tbname = "", string autoname = "", List<P> paralist = null) where T : BaseEntity where P : IDataParameter, new()
        {

            //如果不是从特性中获取自增列,表名,必须手动给值
            string sqlstr = string.Empty;

            //sqlpre 后续做成枚举强类型,暂时只考虑Mysql的@

            //P 对于参数化的SQL,采用引用类型传引用处理

            if (t == null || t.ChangeDic == null || t.ChangeDic.Count <= 0)
            {
                return "";
            }
            sqlstr += string.Format(@"insert into  {0}  ", tbname);

            string fields = string.Empty;
            string values = string.Empty;

            foreach (KeyValuePair<string, object> item in t.ChangeDic)
            {
                if (!string.IsNullOrEmpty(autoname))
                {

                    if (item.Key == autoname)
                    {
                        continue;
                    }
                }
                fields += item.Key + ",";
                values += sqlpre + item.Key + ",";


                if (paralist != null)
                {

                    paralist.Add(new P() { ParameterName = item.Key, Value = item.Value == null ? DBNull.Value : item.Value });
                }

            }
            fields = fields.Trim(',');
            values = values.Trim(',');
            sqlstr = sqlstr + string.Format(@"({0})values({1})", fields, values);

            return sqlstr;

        }


        #region 同步程序项目所用
        /// <summary>
        /// 获取匹配的条件
        /// wwj 使用reader取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="temp1">目标字段</param>
        /// <param name="temp2">源字段</param>
        /// <param name="model"></param>
        /// <returns></returns>
        //public static Dictionary<string, object> GetWhereMap<P>(string temp1, string temp2, P reader) where P : IDataReader
        //{
        //    Dictionary<string, object> sourcemap = new Dictionary<string, object>();
        //    Dictionary<string, string> tempmap = new Dictionary<string, string>();
        //    //对应约束
        //    if (temp1.Contains(",") && temp2.Contains(","))
        //    {
        //        string[] targetkeys = temp1.Split(',');
        //        string[] sourekeys = temp2.Split(',');
        //        if (targetkeys.Length == sourekeys.Length)
        //        {
        //            for (int i = 0; i < targetkeys.Length; i++)
        //            {
        //                tempmap.Add(targetkeys[i], sourekeys[i]);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        tempmap.Add(temp1, temp2);
        //    }
        //    object obj = null;
        //    foreach (KeyValuePair<string, string> item in tempmap)
        //    {
        //        try
        //        {
        //            obj = reader[item.Value];
        //        }
        //        catch (Exception)
        //        {

        //            continue;
        //        }


        //        sourcemap.Add(item.Key, obj);
        //    }
        //    tempmap.Clear();
        //    tempmap = null;
        //    return sourcemap;
        //}


        /// <summary>
        /// 获取Where条件
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        //public static string GetWhereConditionMap(Dictionary<string, object> dic)
        //{
        //    string result = string.Empty;

        //    if (dic != null && dic.Count > 0)
        //    {
        //        string tempsql = string.Empty;
        //        //object obj = null;
        //        //if (sourcevalues.Contains(","))
        //        //{

        //        foreach (KeyValuePair<string, object> item in dic)
        //        {
        //            //if (dic.ContainsKey(item))
        //            //{
        //            //obj = item.Value;
        //            if (item.Value != null)
        //            {
        //                tempsql += " " + item.Key + "='" + item.Value.ToString().Trim('\'') + "' " + "and";
        //            }
        //            //}

        //        }
        //        if (!string.IsNullOrEmpty(tempsql))
        //        {
        //            tempsql = tempsql.Remove(tempsql.LastIndexOf("and"));
        //        }

        //        //}
        //        //else
        //        //{
        //        //    if (dic.ContainsKey(sourcevalues))
        //        //    {
        //        //        obj = dic[sourcevalues];
        //        //        if (obj != null)
        //        //        {
        //        //            tempsql = " " + sourcevalues + "='" + obj.ToString().Trim('\'') + "' ";
        //        //        }
        //        //    }


        //        //}
        //        if (!string.IsNullOrEmpty(tempsql))
        //        {
        //            result = tempsql;
        //        }
        //    }





        //    return result;



        //}


        /// <summary>
        /// 从实体中找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourcevalues"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        //public static string GetWhereCondition<T>(string sourcevalues, T t)
        //{
        //    string result = string.Empty;

        //    if (!string.IsNullOrEmpty(sourcevalues))
        //    {
        //        string tempsql = string.Empty;
        //        object obj = null;
        //        if (sourcevalues.Contains(","))
        //        {

        //            foreach (var item in sourcevalues.Split(','))
        //            {
        //                obj = ModelFe.GetIntance().ReturnAttValue(t, item);//先加载程序集wwj
        //                if (obj != null)
        //                {
        //                    tempsql += " " + item + "='" + obj.ToString().Trim('\'') + "' " + "and";
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(tempsql))
        //            {
        //                tempsql = tempsql.Remove(tempsql.LastIndexOf("and"));
        //            }

        //        }
        //        else
        //        {
        //            obj = ModelFe.GetIntance().ReturnAttValue(t, sourcevalues);
        //            if (obj != null)
        //            {
        //                tempsql = " " + sourcevalues + "='" + obj.ToString().Trim('\'') + "' ";
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(tempsql))
        //        {
        //            result = tempsql;
        //        }
        //    }





        //    return result;



        //}


        /// <summary>
        /// 获取匹配的条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //public static Dictionary<string, object> GetWhereMapByModel<T>(string temp1, string temp2, T model) where T : BaseEntity
        //{
        //    Dictionary<string, object> sourcemap = new Dictionary<string, object>();
        //    Dictionary<string, string> tempmap = new Dictionary<string, string>();
        //    //对应约束
        //    if (temp1.Contains(",") && temp2.Contains(","))
        //    {
        //        string[] targetkeys = temp1.Split(',');
        //        string[] sourekeys = temp2.Split(',');
        //        if (targetkeys.Length == sourekeys.Length)
        //        {
        //            for (int i = 0; i < targetkeys.Length; i++)
        //            {
        //                tempmap.Add(targetkeys[i], sourekeys[i]);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        tempmap.Add(temp1, temp2);
        //    }

        //    foreach (KeyValuePair<string, string> item in tempmap)
        //    {
        //        sourcemap.Add(item.Key, ModelFe.GetIntance().ReturnAttValue(model, item.Value));
        //    }
        //    tempmap.Clear();
        //    tempmap = null;
        //    return sourcemap;
        //}

        #endregion










    }
}
