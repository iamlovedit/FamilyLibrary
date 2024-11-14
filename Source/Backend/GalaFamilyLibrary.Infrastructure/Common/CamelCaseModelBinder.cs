using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GalaFamilyLibrary.Infrastructure.Common;

public class CamelCaseModelBinderProvider:IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        throw new NotImplementedException();
    }
}

public class CamelCaseModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var model = Activator.CreateInstance(bindingContext.ModelType);
        var propertyNames = bindingContext.ModelType.GetProperties().Select(p => ConvertToCamelCase(p.Name));

        foreach (var propertyName in propertyNames)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(propertyName);
            if (valueResult == ValueProviderResult.None)
            {
                continue;
            }
            var property = bindingContext.ModelType.GetProperty(ConvertToToPascalCase(propertyName));
            property.SetValue(model, valueResult.FirstValue);
        }

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }

    private static string ConvertToCamelCase(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        return Regex.Replace(source, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1", RegexOptions.Compiled)
            .Trim()
            .Replace(" ", string.Empty)
            .ToLower();
    }

    private static string ConvertToToPascalCase(string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        return Regex.Replace(source, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1", RegexOptions.Compiled)
            .Trim()
            .Replace(" ", string.Empty)
            .ToLower()
            .Substring(0, 1)
            .ToUpper() + source.Substring(1);
    }
}