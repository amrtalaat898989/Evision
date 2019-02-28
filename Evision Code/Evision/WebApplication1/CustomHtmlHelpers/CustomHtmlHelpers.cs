using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebApplication1.CustomHtmlHelpers
{
    public static class CustomHtmlHelpers
    {

        public static IHtmlString Image(this HtmlHelper hepler, string src)
        {
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("width", "40");
            tb.Attributes.Add("height", "40");

            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }

    }
}