using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cts.WebApp.Platform.RazorHelpers
{
    /// <summary>
    /// <see cref="ITagHelper"/> implementation targeting &lt;label&gt; elements with an <c>asp-for</c> attribute.
    /// </summary>
    [HtmlTargetElement("label", Attributes = ForAttributeName)]
    public class LabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [UsedImplicitly]
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression Model { get; set; } = default!;

        /// <inheritdoc />
        /// <remarks>Adds text indicating the field is required if the property has the RequiredAttribute.</remarks>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Model.Metadata.IsRequired && Model.Metadata.ModelType != typeof(bool) && !Model.Metadata.ModelType.IsEnum)
                output.Content.AppendHtml(@" <span class=""text-danger"">*</span>");
        }
    }
}
