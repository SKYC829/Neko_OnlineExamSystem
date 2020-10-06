using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neko.Unity
{
    public static class PageSizeHelper
    {
        public static IHtmlContent Page(string controllerName, string actionName, int total, int pageIndex, int totalPage)
        {
            return Page(null, controllerName, actionName, total, pageIndex, totalPage);
        }
        public static IHtmlContent Page(string areaName,string controllerName,string actionName,int total,int pageIndex,int totalPage)
        {
            string url = string.Format("/{0}/{1}", controllerName, actionName);
            if (!string.IsNullOrEmpty(areaName))
            {
                url = string.Format("/{0}/{1}/{2}", areaName, controllerName, actionName);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<nav class=\"justify-content-end \">");
            sb.Append("<ul class=\"pagination \">");
            sb.Append("<li class=\"page-item" + (pageIndex == 1 ? " disabled" : "") + "\">");
            sb.Append(string.Format("<a class=\"page-link\" href=\"{0}?pageIndex=1\">第一页</a>", url));
            sb.Append("</li>");
            sb.Append("<li class=\"page-item" + (pageIndex == 1 ? " disabled" : "") + "\">");
            sb.Append(string.Format("<a class=\"page-link\" href=\"{0}?pageIndex={1}\">上一页</a>", url, pageIndex == 1 ? 1 : pageIndex - 1));
            sb.Append("</li>");
            for (int i = 0; i < totalPage; i++)
            {
                sb.Append(string.Format("<li class=\" page-item {0}\">", pageIndex == (i+1) ? "active" : ""));
                sb.Append(string.Format("<a class=\"page-link\" href={0}?pageIndex={1}>{1}</a>", url, (i+1)));
                sb.Append("</li>");
            }
            sb.Append("<li class=\"page-item" + (pageIndex == totalPage ? " disabled" : "") + "\">");
            sb.Append(string.Format("<a class=\"page-link\" href=\"{0}?pageIndex={1}\">下一页</a>", url, pageIndex == totalPage ? totalPage : pageIndex + 1));
            sb.Append("</li>");
            sb.Append("<li class=\"page-item" + (pageIndex == totalPage ? " disabled" : "") + "\">");
            sb.Append(string.Format("<a class=\"page-link\" href=\"{0}?pageIndex={1}\">最后一页</a>", url, totalPage));
            sb.Append("</li>");
            sb.Append("</ul>");
            sb.Append("</nav>");
            if(total <= 0)
            {
                return new HtmlString("");
            }
            return new HtmlString(sb.ToString());
        }
    }
}
