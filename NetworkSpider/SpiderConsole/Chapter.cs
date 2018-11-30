using HtmlAgilityPack;
using Model;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SpiderConsole
{
    public class Chapter
    {
        private static readonly string ChapterXpath = "/a[1]";

        private Chapter() { }

        public static Chapter CreatChapterForHtml(string html)
        {
            Chapter chapter = new Chapter();
            chapter.HtmlDocument.LoadHtml(html);
            return chapter;
        }

        private string _name;
        /// <summary>
        /// 章节名
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = GetDocumentNodeHtml(ChapterXpath).Attributes["title"].Value;
                }
                return _name;
            }
        }

        private string _url;
        /// <summary>
        /// 链接
        /// </summary>
        public string Url
        {
            get
            {
                if (string.IsNullOrEmpty(_url))
                    _url = DmzjWebSpider.DownloadUrl + GetDocumentNodeHtml(ChapterXpath).Attributes["href"].Value;
                return _url;
            }
        }

        private List<ComicPage> _comicPageList=new List<ComicPage>();
        /// <summary>
        /// 详细页
        /// </summary>
        public List<ComicPage> ComicPageList
        {
            get
            {
                if (_comicPageList.Count <=0)
                {
                    GetChapterComicPage();
                }
                return _comicPageList;
            }

        }

        public HtmlDocument HtmlDocument { get; set; } = new HtmlDocument();

        

        public HtmlNode GetDocumentNodeHtml(string xpath)
        {
            return HtmlDocument.DocumentNode.SelectSingleNode(xpath);
        }

        private void GetChapterComicPage()
        {
            var service = PhantomJSDriverService.CreateDefaultService();
            service.DiskCache = true;
            service.IgnoreSslErrors = true;
            service.HideCommandPromptWindow = true;
            service.LoadImages = false;
            service.ProxyType = "none";
            service.LocalToRemoteUrlAccess = true;
            PhantomJSDriver driver = new PhantomJSDriver(service, new PhantomJSOptions(), TimeSpan.FromSeconds(120));
            driver.Navigate().GoToUrl(Url);

            var element = driver.FindElementById("page_select");
            foreach (var pages in driver.FindElements(By.XPath("//*[@id=\"page_select\"]/option")))
            {
                var page = new ComicPage();
                page.PageImgUrl = "https:" + pages.GetAttribute("value");
                page.PageNo = pages.Text.GetNumber();
                _comicPageList.Add(page);
            }
            driver.Close();
            driver.Dispose();
        }

        public void Dounload(string path)
        {
            path += "\\" + Name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ComicPageList.OrderBy(m => m.PageNo);
            foreach (var page in ComicPageList)
            {
                WebClient webrequest = new WebClient();
                webrequest.Headers.Add("ContentType", "image/jpeg");
                webrequest.Headers.Add("Method", "GET");
                webrequest.Headers.Add("Referer", Url);
                webrequest.DownloadFile(page.PageImgUrl, path + "\\" + page.PageNo + page.PageImgUrl.Substring(page.PageImgUrl.LastIndexOf('.'), page.PageImgUrl.Length - page.PageImgUrl.LastIndexOf('.')));
            }
        }

    }
}
