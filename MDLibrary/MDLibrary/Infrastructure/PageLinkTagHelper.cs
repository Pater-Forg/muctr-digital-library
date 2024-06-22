using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MDLibrary.Models;
using System.Linq;

namespace MDLibrary.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

#pragma warning disable CS8618
        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
#pragma warning restore CS8618
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }

        public string PageAction { get; set; }

        public IQueryCollection PageQuery { get; set; }

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public TagBuilder BuildPageTag(string action, string text)
        {
			var tag = new TagBuilder("a");
			if (PageClassesEnabled)
				tag.AddCssClass(PageClass);
			tag.Attributes["href"] = action;
			tag.InnerHtml.Append(text);
            return tag;
		}

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");
            var queryDict = PageQuery.ToDictionary(p => p.Key, p => p.Value.FirstOrDefault());

			if (PageModel.TotalPages <= 8)
			{
				for (int i = 1; i <= PageModel.TotalPages; i++)
				{
					queryDict["p"] = i.ToString();
					var tag = BuildPageTag(urlHelper.Action(PageAction, queryDict), i.ToString());
					if (PageClassesEnabled)
					{
						tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
					}
					result.InnerHtml.AppendHtml(tag);
				}
				output.Content.AppendHtml(result.InnerHtml);
				return;
			}

			var prevPage = PageModel.CurrentPage != 1 ? PageModel.CurrentPage - 1 : 1;
			queryDict["p"] = prevPage.ToString();
			var tagPrevious = BuildPageTag(urlHelper.Action(PageAction, queryDict), "");
			tagPrevious.InnerHtml.AppendHtml("<i class=\"fa-solid fa-angle-left\"></i>");
			result.InnerHtml.AppendHtml(tagPrevious);

			if (PageModel.CurrentPage < 4)
            {
                for (int i = 1; i <= 4 && i <= PageModel.TotalPages; i++)
                {
					queryDict["p"] = i.ToString();
                    var tag = BuildPageTag(urlHelper.Action(PageAction, queryDict), i.ToString());
					if (PageClassesEnabled)
					{
						tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
					}
					result.InnerHtml.AppendHtml(tag);
				}
				var tagMiddle = BuildPageTag("#", "...");
                tagMiddle.AddCssClass(PageClassNormal);
				queryDict["p"] = PageModel.TotalPages.ToString();
				var tagLast = BuildPageTag(urlHelper.Action(PageAction, queryDict), queryDict["p"]);
				tagLast.AddCssClass(PageClassNormal);
				result.InnerHtml.AppendHtml(tagMiddle);
				result.InnerHtml.AppendHtml(tagLast);
			}
            else if (PageModel.CurrentPage > PageModel.TotalPages - 3)
            {
                queryDict["p"] = "1";
                var tagFirst = BuildPageTag(urlHelper.Action(PageAction, queryDict), "1");
				tagFirst.AddCssClass(PageClassNormal);
				var tagMiddle = BuildPageTag("#", "...");
				tagMiddle.AddCssClass(PageClassNormal);
				result.InnerHtml.AppendHtml(tagFirst);
				result.InnerHtml.AppendHtml(tagMiddle);
				for (int i = PageModel.TotalPages - 3; i <= PageModel.TotalPages; i++)
				{
					queryDict["p"] = i.ToString();
					var tag = BuildPageTag(urlHelper.Action(PageAction, queryDict), i.ToString());
					if (PageClassesEnabled)
					{
						tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
					}
					result.InnerHtml.AppendHtml(tag);
				}
			}
            else
            {
				queryDict["p"] = "1";
				var tagFirst = BuildPageTag(urlHelper.Action(PageAction, queryDict), "1");
				tagFirst.AddCssClass(PageClassNormal);
				var tagMiddle = BuildPageTag("#", "...");
				tagMiddle.AddCssClass(PageClassNormal);
				result.InnerHtml.AppendHtml(tagFirst);
				result.InnerHtml.AppendHtml(tagMiddle);
				for (int i = PageModel.CurrentPage - 1; i <= PageModel.CurrentPage + 1; i++)
				{
					queryDict["p"] = i.ToString();
					var tag = BuildPageTag(urlHelper.Action(PageAction, queryDict), i.ToString());
					if (PageClassesEnabled)
					{
						tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
					}
					result.InnerHtml.AppendHtml(tag);
				}
				result.InnerHtml.AppendHtml(tagMiddle);
				queryDict["p"] = PageModel.TotalPages.ToString();
				var tagLast = BuildPageTag(urlHelper.Action(PageAction, queryDict), queryDict["p"]);
				tagLast.AddCssClass(PageClassNormal);
				result.InnerHtml.AppendHtml(tagLast);
			}

			var nextPage = PageModel.CurrentPage != PageModel.TotalPages
				? PageModel.CurrentPage + 1
				: PageModel.TotalPages;
			queryDict["p"] = nextPage.ToString();
			var tagNext = BuildPageTag(urlHelper.Action(PageAction, queryDict), "");
			tagNext.InnerHtml.AppendHtml("<i class=\"fa-solid fa-angle-right\"></i>");
			result.InnerHtml.AppendHtml(tagNext);

			output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
