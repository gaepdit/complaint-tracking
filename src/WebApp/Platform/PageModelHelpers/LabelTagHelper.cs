using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cts.WebApp.Platform.PageModelHelpers;

/// <summary>
/// <see cref="ITagHelper"/> implementation targeting <c>&lt;label&gt;</c> elements with an <c>asp-for</c> attribute.
/// </summary>
[HtmlTargetElement("label", Attributes = ForAttributeName)]
public class LabelTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";

    private ModelExpression? _model;

    /// <summary>
    /// An expression to be evaluated against the current model.
    /// </summary>
    [UsedImplicitly]
    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression Model
    {
        set => _model = value;
        get => _model ?? throw new InvalidOperationException("Uninitialized Model.");
    }

    /// <inheritdoc />
    /// <remarks>Adds text indicating the field is required if the property has the RequiredAttribute.</remarks>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model.Metadata.IsRequired && Model.Metadata.ModelType != typeof(bool) && !Model.Metadata.ModelType.IsEnum)
            output.Content.AppendHtml(""" <span class="text-danger-emphasis">*</span>""");
    }
}
