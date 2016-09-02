using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

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

            if ((string.IsNullOrWhiteSpace(ControllerName) ||
                string.Compare(ControllerName, controller, StringComparison.OrdinalIgnoreCase) == 0) &&
                string.Compare(ActionName, action, StringComparison.OrdinalIgnoreCase) == 0)
            {
                var classValue = output.Attributes.ContainsName("class") ? $"{output.Attributes["class"]} {ClassName}" : ClassName;

                output.Attributes.SetAttribute("class", classValue);
            }

            base.Process(context, output);
        }
    }
}
