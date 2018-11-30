using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiderConsole
{
    public class DmzjWebSpider
    {
        public static readonly string DownloadUrl = "https://manhua.dmzj.com/";
        private static readonly string SearchUrl = "https://www.dmzj.com/dynamic/o_search/index";
        //private static readonly string downloadPath = @"H:\OneDrive\OneDrive - nikaidouyui\漫画";       //Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\下载";
        private static readonly string searchCountXpath = "/html/body/div[2]/div[1]/div[1]/div/span/em";
        private static readonly string cartoonXapth = "/html/body/div[2]/div[1]/div[2]/div/ul/li";

        public static readonly string downloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\下载";

        public HtmlDocument HtmlDocument { get; set; } = new HtmlDocument();

        public string KeyWords { get; set; }

        private DmzjWebSpider() { }

        public static DmzjWebSpider GetDmzjWebSpiderForKeyWords(string keywords)
        {
            DmzjWebSpider spider = new DmzjWebSpider();
            spider.KeyWords = keywords;
            spider.HtmlDocument.LoadHtml(spider.GetSearchHtml());
            return spider;
        }

        public string GetSearchHtml()
        {
            return HttpPostHelper.HttpPost(SearchUrl, "keywords=" + KeyWords);
        }


        public string GetDocumentNodeHtml(string xpath)
        {
            return HtmlDocument.DocumentNode.SelectSingleNode(xpath).InnerHtml;
        }

        public List<Cartoon> GetCartoons()
        {
            HtmlNodeCollection cartoonNodes = HtmlDocument.DocumentNode.SelectNodes(cartoonXapth);
            return cartoonNodes.Select(x => Cartoon.CreatCartoonForHtml(x.InnerHtml)).ToList();
        }

        public int GetSearchCount()
        {
            return int.Parse(GetDocumentNodeHtml(searchCountXpath));
        }
    }
}
