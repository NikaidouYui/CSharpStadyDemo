using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程1");
            Write1();
            Console.WriteLine("主线程2");
            Console.ReadKey();
        }


        async static void Write1()
        {
            Console.WriteLine("开始执行子函数!");
            Thread.Sleep(6000);            
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("开始执行异步Task!");
                Thread.Sleep(5000);
                Console.WriteLine("异步Task执行完毕!");
            });
            Console.WriteLine("子函数继续执行");
        }


    }
}
