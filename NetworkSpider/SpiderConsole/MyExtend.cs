using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public static  class MyExtend
    {

        public static int GetNumber(this string oldString)
        {
           return int.Parse(System.Text.RegularExpressions.Regex.Replace(oldString, @"[^0-9]+", ""));
        }

        /// <summary>
        /// 检查list是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> CheckNull<T>(this IEnumerable<T> list)
        {
            return list == null ? new List<T>(0) : list;
        }

        /// <summary>
        /// 判断字符串是否有汉字字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasAnyHanZi(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] > 127)
                    return true;
            }
            return false;
        }
    }
}
