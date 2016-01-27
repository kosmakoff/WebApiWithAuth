using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiWithAuth.TagHelpers
{
    [HtmlTargetElement("li", Attributes = ActionNameAttributeName)]
    public class RouteClassTagHelper : TagHelper
    {
        private const string ControllerNameAttributeName = "controller-name";
        private const string ActionNameAttributeName = "action-name";
        private const string ClassNameAttributeName = "class-name";

        [HtmlAttributeName(ControllerNameAttributeName)]
        public string ControllerName { get; set; }

        [HtmlAttributeName(ActionNameAttributeName)]
        public string ActionName { get; set; }

        [HtmlAttributeName(ClassNameAttributeName)]
        public string ClassName { get; set; } = "active";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var controller = ViewContext.RouteData.Values["controller"] as string;
            var action = ViewContext.RouteData.Values["action"] as string;

            if ((string.IsNullOrWhiteSpace(ControllerName) || string.Compare(ControllerName, controller, true) == 0) &&
                string.Compare(ActionName, action, true) == 0)
            {
                string classValue;

                if (output.Attributes.ContainsName("class"))
                {
                    classValue = $"{output.Attributes["class"]} {ClassName}";
                }
                else
                {
                    classValue = ClassName;
                }

                output.Attributes["class"] = classValue;
            }

            base.Process(context, output);
        }
    }
}
