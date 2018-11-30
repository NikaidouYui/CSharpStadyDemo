using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;
using Model;

namespace SpiderConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("请输入想要搜索的关键字.");
            DmzjWebSpider spider = DmzjWebSpider.GetDmzjWebSpiderForKeyWords(Console.ReadLine());
            string searchResult = "共找到" + spider.GetSearchCount() + "条结果!";
            if (spider.GetSearchCount()==0)
            {
                Console.WriteLine("未能找到对应的漫画!");
                Console.ReadKey();
                return;
            }
            var cartoons = spider.GetCartoons();
            PrintComicInfo(cartoons);

            Console.WriteLine();
            var selectCartoon = cartoons[GetSelectCartoomIndex(cartoons.Count)];
            Console.WriteLine("当前所选漫画共有" + selectCartoon.Chapters.Count + "章!");
            for (int i = 1; i <= selectCartoon.Chapters.Count; i++)
            {
                Console.WriteLine("[" + i.ToString() + "]." + selectCartoon.Chapters[i - 1].Name);
            }
            int chapterIndex = GetSelectChapterIndex(selectCartoon.Chapters.Count);
            if (chapterIndex != 0)
            {
                selectCartoon.Download(chapterIndex - 1);
            }
            else
            {
                selectCartoon.Download();
            }
            Console.WriteLine("下载完成!");
            Console.ReadKey();
        }


        /// <summary>
        /// 获取选取章节的索引
        /// </summary>
        /// <param name="chaptersCount"></param>
        /// <returns></returns>
        private static int GetSelectChapterIndex(int chaptersCount)
        {
           int selectNo = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine("请输入你想下载的章节!0为全部下载!");
                    selectNo = int.Parse(Console.ReadLine());
                    if (selectNo < 0 || selectNo > chaptersCount)
                    {
                        Console.WriteLine("请输入正确范围的序号!");
                        continue;
                    }
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("请输入正确的序号!");
                    Console.WriteLine();
                    continue;
                }
            }
            return selectNo;
        }

        /// <summary>
        /// 获取选取漫画的索引
        /// </summary>
        /// <param name="cartoonsCount"></param>
        /// <returns></returns>
        private static int GetSelectCartoomIndex(int cartoonsCount)
        {
            int selectNo = -1;
            while (true)
            {
                try
                {
                    Console.WriteLine("请输入需要进入的漫画序号!");
                    selectNo = int.Parse(Console.ReadLine());
                    if (selectNo < 0 || selectNo > cartoonsCount)
                    {
                        Console.WriteLine("请输入正确范围的序号!");
                        continue;
                    }
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("请输入正确的序号!");
                    Console.WriteLine();
                    continue;
                }
            }
            return selectNo - 1;
        }

        /// <summary>
        /// 显示漫画的详细详细
        /// </summary>
        /// <param name="cartoons"></param>
        private static void PrintComicInfo(List<Cartoon> cartoons)
        {
            for (int i = 0; i < cartoons.Count; i++)
            {
                Console.WriteLine("序号:[" + (i+1).ToString("00") + "]");
                Console.WriteLine("漫画名:" + cartoons[i].Name);
                Console.WriteLine("作者:" + cartoons[i].Auther);
                Console.WriteLine(cartoons[i].NewChapter);
                Console.WriteLine("是否完结:" + (cartoons[i].IsOverComic ? "已完结" : "未完结"));
                Console.WriteLine("漫画链接:" + cartoons[i].ComicUrl);
                Console.WriteLine();
            }
        }

    }
}
