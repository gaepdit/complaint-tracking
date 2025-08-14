using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Cts.WebApp.Platform.PageModelHelpers;

/// <summary>
/// This <see cref="ITagHelper"/> implementation adds the Bootstrap compatible class "is-invalid" to form elements
/// <c>input</c>, <c>select</c>, and <c>textarea</c> if they have already been marked as invalid with the ASP.NET
/// default class "input-validation-error".
/// </summary>
[HtmlTargetElement("input", Attributes = ForAttributeName)]
[HtmlTargetElement("select", Attributes = ForAttributeName)]
[HtmlTargetElement("textarea", Attributes = ForAttributeName)]
public class InvalidClassTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";
    private const string ClassAttributeName = "class";
    private const string BootstrapInvalidElementClass = "is-invalid";

    // If the element contains the default ASP.NET class indicating a validation error,
    // this will add the corresponding Bootstrap class.
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);

        if (output.Attributes.TryGetAttribute(ClassAttributeName, out var classAttribute) &&
            ExtractClassValue(classAttribute).Contains(HtmlHelper.ValidationInputCssClassName))
        {
            output.AddClass(BootstrapInvalidElementClass, HtmlEncoder.Default);
        }
    }

    // Gets the current value of the element's class attribute.
    // Adapted from https://github.com/dotnet/aspnetcore/blob/ab9e2630e6144efc529bfa8c67caa68732c80086/src/Mvc/Mvc.TagHelpers/src/TagHelperOutputExtensions.cs#L257-L283
    private static string ExtractClassValue(TagHelperAttribute classAttribute)
    {
        string? extractedClassValue;

        switch (classAttribute.Value)
        {
            case string valueAsString:
                extractedClassValue = HtmlEncoder.Default.Encode(valueAsString);
                break;
            case HtmlString valueAsHtmlString:
                extractedClassValue = valueAsHtmlString.Value;
                break;
            case IHtmlContent valueAsHtmlContent:
                using (var stringWriter = new StringWriter())
                {
                    valueAsHtmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
                    extractedClassValue = stringWriter.ToString();
                }

                break;
            default:
                extractedClassValue = null;
                break;
        }

        return extractedClassValue ?? string.Empty;
    }
}
