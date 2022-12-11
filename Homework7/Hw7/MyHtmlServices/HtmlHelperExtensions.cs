using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    private static TagBuilder CreateLabel(PropertyInfo property)
    {
        var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();

        var labelTagBuilder = new TagBuilder("label");
        labelTagBuilder.MergeAttribute("for", property.Name);
        labelTagBuilder.MergeAttribute("style", "padding: 10px");

        if (displayAttribute == null)
            labelTagBuilder.InnerHtml.AppendHtmlLine(SplitCamelCase(property.Name));
        else
            labelTagBuilder.InnerHtml.AppendHtmlLine(displayAttribute.Name);

        return labelTagBuilder;
    }
    
    private static string SplitCamelCase(string str)
        => string.Concat(str.Select((x,i) => i > 0 && char.IsUpper(x) ? " " + x.ToString() : x.ToString()));
    
    private static TagBuilder CreateEntryField(PropertyInfo property, object? model)
    {
        TagBuilder entryTagBuilder;

        if (property.PropertyType.IsEnum)
        {
            entryTagBuilder = new TagBuilder("select");
            foreach (var enumName in property.PropertyType.GetEnumNames())
            {
                var optionTagBuilder = new TagBuilder("option");
                optionTagBuilder.InnerHtml.AppendHtmlLine(enumName);
                entryTagBuilder.InnerHtml.AppendHtml(optionTagBuilder);
            }
        }
        else
        {
            entryTagBuilder = new TagBuilder("input");
            var type = property.PropertyType switch
            {
                Type t when t == typeof(int) => "number",
                Type t when t == typeof(string) => "text",
                _ => "undefined"
            };
            entryTagBuilder.MergeAttribute("type", type);
        }
        
        entryTagBuilder.GenerateId(property.Name, "?");

        return entryTagBuilder;
    }

    private static TagBuilder CreateSpan(PropertyInfo property, object? model)
    {
        var spanTagBuilder = new TagBuilder("span");
        spanTagBuilder.MergeAttribute("style", "padding: 10px");

        if (model != null)
        {
            foreach (var validationAttribute in property.GetCustomAttributes<ValidationAttribute>())
            {
                if (!validationAttribute.IsValid(property.GetValue(model)))
                    spanTagBuilder.InnerHtml.AppendHtmlLine(validationAttribute.ErrorMessage);
            }
        }

        return spanTagBuilder;
    }
    
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var properties = helper.ViewData.ModelMetadata.ModelType.GetProperties();
        var divTagBuilder = new TagBuilder("div");
        
        foreach (var property in properties)
        {
            var subdivTagBuilder = new TagBuilder("div");

            subdivTagBuilder.InnerHtml.AppendHtml(CreateLabel(property));
            subdivTagBuilder.InnerHtml.AppendHtml(CreateEntryField(property, model));
            subdivTagBuilder.InnerHtml.AppendHtml(CreateSpan(property, model));

            divTagBuilder.InnerHtml.AppendHtml(subdivTagBuilder);
        }

        return divTagBuilder;
    }
} 