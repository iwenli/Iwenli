using Iwenli.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Test2
{
    public class Product
    {
        public string Name { get; set; }
        public int Code { get; set; }
    }
    /// <summary>
    /// Distinct扩展集合
    /// </summary>
    public static class DistinctExtensions
    {
        #region Distinct
        /// <summary>
        /// Distinct扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct1<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
        //扩展方法
        /// <summary>
        /// 方法2
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct2<TSource>
            (this IEnumerable<TSource> source, Func<TSource, TSource, bool> predicate)
        {
            return source.Distinct(new PredicateEqualityComparer<TSource>(predicate));
        }

        /// <summary>
        /// 方法3
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct3<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.GroupBy(keySelector).Select(g => g.FirstOrDefault());
        }
        /// <summary>
        /// 方法4
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct4<T, V>(this IEnumerable<T> source, Func<T, V> keySelector, IEqualityComparer<V> comparer)
        {
            return source.GroupBy(keySelector, comparer).Select(g => g.FirstOrDefault());
        }
        // 加上Linq to XXX的支持
        /// <summary>
        /// 方法5
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IQueryable<T> Distinct5<T, V>(this IQueryable<T> source, Expression<Func<T, V>> keySelector)
        {
            return source.GroupBy(keySelector).Select(g => g.FirstOrDefault());
        } 
        #endregion
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region DistinctExtensions 测试
            Product[] products =
                {
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                 new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "orange", Code = 4 },
                new Product { Name = "apple", Code = 9 },
                new Product { Name = "lemon", Code = 12 },
                new Product { Name = "lemon", Code = 12 }

            };
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            Console.WriteLine("原数据：");
            foreach (Product pro in products)
                Console.WriteLine(pro.Name + "," + pro.Code);

            stopwatch.Start();
            int i = 10;
            var distinctProducts3 = products.Distinct3(p => p.Code);
            i = 1000;
            while (i-- > 0)
            {
                distinctProducts3 = products.Distinct3(p => p.Name);
            }
            stopwatch.Stop(); //  停止监视
            Console.WriteLine(string.Format("去重<方法3>执行1000次,耗时{0}毫秒,结果如下：", stopwatch.Elapsed.TotalMilliseconds));
            foreach (Product pro in distinctProducts3)
            {
                Console.WriteLine(pro.Name + "," + pro.Name);
            }
            stopwatch.Restart();
            var distinctProducts1 = products.Distinct1(p => p.Name);
            i = 1000;
            while (i-- > 0)
            {
                distinctProducts1 = products.Distinct1(p => p.Name);
            }
            stopwatch.Stop(); //  停止监视
            Console.WriteLine(string.Format("去重<方法1>执行1000次,耗时{0}毫秒,结果如下：", stopwatch.Elapsed.TotalMilliseconds));
            foreach (Product pro in distinctProducts1)
            {
                Console.WriteLine(pro.Name + "," + pro.Name);
            }

            stopwatch.Restart();
            var distinctProducts2 = products.Distinct2((p1, p2) => p1.Name == p2.Name);
            i = 1000;
            while (i-- > 0)
            {
                distinctProducts2 = products.Distinct2((p1, p2) => p1.Name == p2.Name);
            }
            stopwatch.Stop(); //  停止监视
            Console.WriteLine(string.Format("去重<方法2>执行1000次,耗时{0}毫秒,结果如下：", stopwatch.Elapsed.TotalMilliseconds));
            foreach (Product pro in distinctProducts2)
            {
                Console.WriteLine(pro.Name + "," + pro.Name);
            }

            //方法二 效果最棒  方法一效果次之  方法三最耗时



            Console.ReadKey(); 
            #endregion
        }
    }
}
