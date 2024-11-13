using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;

namespace AdminSenyun.Core.Extensions;

public static class CoreHelp
{
    public static RenderFragment UI<T>(this IComponent component, Func<T, T> action) where T : IComponent, new()
    {
        RenderFragment render = builder =>
        {
            builder.OpenComponent<T>(0);
            var t = new T();
            var x = action(t);
            builder.AddContent(1, x);
            builder.CloseComponent();
        };
        return render;
    }

    public static Compont<T> GetCompont<T>(this T t)
    {
        return new Compont<T>();
    }


    public static Dictionary<string, TValue> DictionaryEx<Tkey, TValue>(this Dictionary<string, TValue> keyValuePairs, Expression<Func<Tkey, TValue>> expression, TValue value)
    {
        var exc = (expression.Body is LambdaExpression lambda) ? expression : throw new Exception("没有获取到表达式");

        keyValuePairs.Remove(exc.Name);

        keyValuePairs[exc.Name] = value;
        return keyValuePairs;
    }


    public static DictionaryEx<T> GetDictionaryEx<T>() where T : class, new()
    {
        return new DictionaryEx<T>();
    }


    public static void Render<TCom>(this TCom com) where TCom : IComponent => com.GetType()?.GetMethod("StateHasChanged", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(com, null);


    public static EventCallback<T> GetEventCallback<T>(this IComponent component, Func<T, Task> callback) => EventCallback.Factory.Create(component, callback);

}


public class DictionaryEx<T> where T : class, new()
{
    private Dictionary<string, object?>? keyValuePairs;
    public Dictionary<string, object?> KeyValues => keyValuePairs ??= [];

    public DictionaryEx<T> Set<TValue>(Expression<Func<T, TValue>> expression, TValue value)
    {
        var exc = (expression.Body is MemberExpression member) ? member : throw new Exception("没有获取到表达式");

        if (KeyValues.ContainsKey(exc.Member.Name))
            KeyValues.Remove(exc.Member.Name);

        KeyValues[exc.Member.Name] = value;
        return this;
    }
    public Dictionary<string, object?>? Build()
    {
        return KeyValues;
    }
}

public class Compont<T>
{
    private readonly RenderTreeBuilder builder;

    public Compont()
    {
        builder = new RenderTreeBuilder();
    }
}