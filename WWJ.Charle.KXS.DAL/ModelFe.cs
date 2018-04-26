using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace WWJ.Charle.KXS.DAL
{
    /// <summary>
    /// 反射model
    /// WWJ
    /// 20180226
    /// </summary>
    public class ModelFe
    {

        private Assembly myAss = null;

      

        private static readonly ModelFe myModelFelManage = new ModelFe();
        private ModelFe()
        {
           
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static ModelFe GetIntance()
        {
            
            return myModelFelManage;//后续优化 把加载也封装进来 wwj
        } 

        /// <summary>
        /// 加载程序集
        /// </summary>
        public void  LoadAss(string assname)
        {
            if (myAss!=null)
            {
                return;
            }
            try
            {
                myAss= Assembly.LoadFrom(assname);
            }
            catch (Exception ex)
            {
               // LoggerManager.Instance.Error(ex);

            }
           
        }

        /// <summary>
        /// 获取model实例
        /// </summary>
        public T GetModel<T>(string modelname)
        {
            try
            {
               return (T)myAss.CreateInstance("BPPC.RMS.DataSync.Model."+ modelname, true);
            }
            catch (Exception ex)
            {
                //LoggerManager.Instance.Error(ex);
                return default(T);
            }

        }


        /// <summary>
        /// 获取实体所有属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public List<string> GetModeAttribute<T>(T t)
        {
            List<string> list = new List<string>();
            Type type = t.GetType();

            PropertyInfo[] pinfos = type.GetProperties();

            if (pinfos!=null&& pinfos.Length>0)
            {
                foreach (PropertyInfo item in pinfos)
                {
                    list.Add(item.Name);
                }
            }

            return list;
        }


        /// <summary>
        /// 反射获取属性值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object ReturnAttValue<T>(T model, string name)
        {
            if (model == null)
            {
                return null;
            }
            object obj = null;
            try
            {
                obj = model.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(model, null);
            }
            catch (Exception ex)
            {
               // LoggerManager.Instance.Error(ex);
                return null;
            }


            return obj;
        }


        /// <summary>
        /// 给实体赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttValue<T>(T model,string name, object value)
        {

            if (model == null)//goto:如果修改空值,需要去掉|| value==nullWWJ20180417
            {
                return;
            }
            if (value==DBNull.Value)
            {
                value = null;//生成脚本时再转换
            }
            try
            {
                model.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).SetValue(model, value,null);
            }
            catch (Exception ex)
            {
                //LoggerManager.Instance.Error(ex);
                return;

            }
        }




    }
}
