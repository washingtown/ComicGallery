using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.ViewModels
{
    public class PaginationModel
    {
        public int StartPage { get; set ; } = 1;
        public int EndPage { get; set; }
        public int CurrentPage { get; set; }
        public int ShowPageLimit { get; set; } = 7;
        public string PreviousText { get; set; } = "‹";
        public string NextText { get; set; } = "›";
        public string FirstPageText { get; set; } = "«";
        public string LastPageText { get; set; } = "»";
        public string Route { get; set; }
        public Dictionary<string, string> RouteParams { get; set; } = new Dictionary<string, string>();
        public List<PaginationItem> Items { get; private set; }

        public PaginationModel()
        {
            CreatePaginationItems();
        }
        public PaginationModel(int maxPage,int currentPage):this()
        {
            EndPage = maxPage;
            CurrentPage = currentPage;
        }
        /// <summary>
        /// 生成页码项
        /// </summary>
        public void CreatePaginationItems()
        {
            var items = new List<PaginationItem>();
            //添加“上一页”按钮
            items.Add(new PaginationItem
            {
                Text = PreviousText,
                TargetPage = CurrentPage - 1,
                Disabled = CurrentPage == StartPage
            });
            //如果总页数不超过显示页数限制，则直接添加所有中间页
            if (EndPage - StartPage < ShowPageLimit)
            {
                items.AddRange(createPagenationItmeOfRange(StartPage, EndPage, CurrentPage));
            }
            else
            {
                //添加第一页
                items.Add(new PaginationItem
                {
                    Text = StartPage.ToString(),
                    TargetPage = StartPage,
                    Active = CurrentPage == StartPage
                });
                int limitHalf = ShowPageLimit / 2;
                if (CurrentPage - StartPage <= limitHalf) //页码较为靠前，则添加前几页
                {
                    items.AddRange(createPagenationItmeOfRange(StartPage + 1, StartPage + ShowPageLimit - 3, CurrentPage));
                    items.Add(new PaginationItem
                    {
                        Text = "···",
                        TargetPage = null,
                        Disabled = true
                    });
                }
                else if (EndPage - CurrentPage <= limitHalf) //页码较为靠后，则添加后几页
                {
                    items.Add(new PaginationItem
                    {
                        Text = "···",
                        TargetPage = null,
                        Disabled = true
                    });
                    items.AddRange(createPagenationItmeOfRange(EndPage - ShowPageLimit + 3, EndPage - 1, CurrentPage));
                }
                else
                {
                    int middleHalf = (ShowPageLimit - 4) / 2;
                    items.Add(new PaginationItem
                    {
                        Text = "···",
                        TargetPage = null,
                        Disabled = true
                    });
                    items.AddRange(createPagenationItmeOfRange(CurrentPage - middleHalf, CurrentPage +middleHalf, CurrentPage));
                    items.Add(new PaginationItem
                    {
                        Text = "···",
                        TargetPage = null,
                        Disabled = true
                    });
                }
                //添加最后一页
                items.Add(new PaginationItem
                {
                    Text = EndPage.ToString(),
                    TargetPage = EndPage,
                    Active = CurrentPage == EndPage
                });
            }
            //添加“下一页”按钮
            items.Add(new PaginationItem
            {
                Text = NextText,
                TargetPage = CurrentPage + 1,
                Disabled = CurrentPage == EndPage
            });
            this.Items = items;
        }
        //生成指定范围的页码项
        private IEnumerable<PaginationItem> createPagenationItmeOfRange(int start,int end,int current)
        {
            return Enumerable.Range(start, end - start + 1)
                .Select(i => new PaginationItem
                {
                    Text = i.ToString(),
                    TargetPage = i,
                    Active = i == current
                });
        }
    }

    public class PaginationItem
    {
        public string Text { get; set; }
        public bool Disabled { get; set; } = false;
        public bool Active { get; set; } = false;
        public int? TargetPage { get; set; }
        public string Href { get; set; }
    }
}
