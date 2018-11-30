using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderConsole
{
    public class Cartoon
    {

        private static readonly string ChapterListXpath = "/html/body/div[3]/div[2]/div[1]/div[4]/ul/li";

        private Cartoon() { }

        public static Cartoon CreatCartoonForHtml(string html)
        {
            Cartoon cartoon = new Cartoon();
            cartoon.HtmlDocument.LoadHtml(html);
            return cartoon;
        }
        public HtmlDocument HtmlDocument { get; set; } = new HtmlDocument();

        private string _name;
        /// <summary>
        /// 漫画名
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                    _name = GetDocumentNodeHtml("./a").Attributes["title"].Value;
                return _name;
            }
        }

        private string _auther;
        /// <summary>
        /// 作者名
        /// </summary>
        public string Auther
        {
            get
            {
                if (string.IsNullOrEmpty(_auther))
                {
                    _auther = GetDocumentNodeHtml("./p[2]").InnerText;
                }
                return _auther;
            }
        }

        private string _newChapter;
        /// <summary>
        /// 最新章节
        /// </summary>
        public string NewChapter
        {
            get
            {
                if (string.IsNullOrEmpty(_newChapter))
                {
                    _newChapter = GetDocumentNodeHtml("./p[3]").InnerText;
                }
                return _newChapter;
            }
        }

        private string _isOverComic;
        /// <summary>
        /// 是否完结
        /// </summary>
        public bool IsOverComic
        {
            get
            {
                if (string.IsNullOrEmpty(_isOverComic))
                {
                    _isOverComic = GetDocumentNodeHtml("./a/p").InnerText ;
                }
                return _isOverComic == "完";
            }
        }

        private string _comicUrl;
        /// <summary>
        /// 漫画链接
        /// </summary>
        public string ComicUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_comicUrl))
                {
                    _comicUrl= "https:" + GetDocumentNodeHtml("./a").Attributes["href"].Value;
                }
                return _comicUrl;
            }
        }

        private List<Chapter> _chapters=new List<Chapter>();
        /// <summary>
        /// 章节列表
        /// </summary>
        public List<Chapter> Chapters
        {
            get
            {
                if (_chapters.Count<=0)
                {
                    _chapters= new HtmlWeb().Load(ComicUrl).DocumentNode.SelectNodes(ChapterListXpath).Select(x => Chapter.CreatChapterForHtml(x.InnerHtml)).ToList();
                }
                return _chapters;
            }
        }



        /// <summary>
        /// 获取对应xpath的html
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public HtmlNode GetDocumentNodeHtml(string xpath)
        {
            return HtmlDocument.DocumentNode.SelectSingleNode(xpath);
        }

        /// <summary>
        /// 下载所有章节
        /// </summary>
        public void Download()
        {
            Chapters.ForEach(x => x.Dounload(DmzjWebSpider.downloadPath + "\\" + Name));
        }

        /// <summary>
        /// 下载所选的章节
        /// </summary>
        /// <param name="chapter"></param>
        public void Download(int chapterIndex)
        {
            Chapters[chapterIndex].Dounload(DmzjWebSpider.downloadPath + "\\" + Name);
        }
    }
}
