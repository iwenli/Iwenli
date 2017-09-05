using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli
{
    /// <summary>
    /// 单例提供者
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class SingletonHelper<T> where T : new()
    {
        /// <summary>
        /// 单例对象
        /// </summary>
        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }


        /// <summary>
        /// 单例创建者
        /// </summary>
        class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T instance = new T();
        }
    }
}
