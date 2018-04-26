using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WWJ.Charle.Model
{
    /// <summary>
    /// 存变化的字段基类
    /// wwj
    /// 20180416
    /// 暂时不用事件的观察者模式
    /// </summary>
    public abstract class BaseEntity:IDisposable
    {
        //从开始到结束这段生成期间

        public BaseEntity()
        {
            ChangeDic = new Dictionary<string, object>();
           // ChangeDic.Clear();//当对象生成时,存对象改变的
        }
        /// <summary>
        /// 普通集合不支持同步
        /// </summary>
        public Dictionary<string,object> ChangeDic { get; set; }//默认值也是NULL

        /// <summary>
        /// 向列表中添加
        /// </summary>
        public void AddList(string name,object value)
        {
            if (ChangeDic.ContainsKey(name))
            {
                ChangeDic[name] = value;
            }
            else
            {
                ChangeDic.Add(name, value);
            }

        }

        public void Dispose()
        {
            ChangeDic.Clear();

        }

        /// <summary>
        /// 移除指定项
        /// </summary>
        public void RemoveList(string name)
        {
            if (ChangeDic.ContainsKey(name))
            {
                ChangeDic.Remove(name);
            }
        }





    }
}
