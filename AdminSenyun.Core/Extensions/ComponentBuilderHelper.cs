using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using System.Reflection;

namespace AdminSenyun.Core.Extensions;

public static class ComponentBuilderHelper
{
    /// <summary>
    /// 渲染树拓展
    /// </summary>
    /// <typeparam name="T">Blazor组件类型</typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ComponentBuilder<T> Component<T>(this RenderTreeBuilder builder) where T : notnull, IComponent
    {
        //返回一个组件建造者类对象，将builder传递给建造者
        //其内部方法需要通过builder来构建组件
        return new ComponentBuilder<T>(builder);
    }
    /// <summary>
    /// 渲染树拓展 自定义div包裹
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="body">子元素</param>
    /// <returns></returns>
    public static ComponentBuilder<ComponentBase> Div(this RenderTreeBuilder builder, RenderFragment body)
    {
        builder.OpenElement(0, "div");
        builder.AddContent(1, body);
        builder.CloseElement();
        return new ComponentBuilder<ComponentBase>(builder);
    }
}

public class ComponentBuilder<T> where T : IComponent
{
    /// <summary>
    /// 渲染树
    /// </summary>
    private readonly RenderTreeBuilder builder;
    /// <summary>
    /// 组件参数字典，设置组件参数时，先存入字典
    /// </summary>
    private readonly Dictionary<string, object> Parameters = new(StringComparer.Ordinal);

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="builder"></param>
    internal ComponentBuilder(RenderTreeBuilder builder)
    {
        this.builder = builder;
    }


    /// <summary>
    ///添加组件参数方法 可以添加非组件定义的属性
    /// </summary>
    /// <param name="name">组件参数名称</param>
    /// <param name="value">组件参数值</param>
    /// <returns></returns>
    public ComponentBuilder<T> Add(string name, object value)
    {
        //将参数存入字典
        Parameters[name] = value;
        //返回this对象，可以流式操作
        return this;
    }

    /// <summary>
    ///设置组件参数方法
    /// </summary>
    /// <typeparam name="TValue">属性的类型</typeparam>
    /// <param name="selector">组件参数属性选择器表达式</param>
    /// <param name="value">参数值</param>
    /// <returns></returns>
    public ComponentBuilder<T> Set<TValue>(Expression<Func<T, TValue>> selector, TValue value)
    {
        //通过属性选择器表达式获取组件参数属性
        var property = Property(selector);
        //添加组件参数
        return Add(property.Name, value);
    }


    public ComponentBuilder<T> Set(Expression<Func<T, EventCallback>> selector, Action callback)
    {
        var property = Property(selector);
        //创建任务
        var call = EventCallback.Factory.Create(this, callback);
        //添加组件参数
        return Add(property.Name, call);
    }
    public ComponentBuilder<T> Set<TValue>(Expression<Func<T, EventCallback<TValue>>> selector, Action callback)
    {
        var property = Property(selector);
        var call = EventCallback.Factory.Create<TValue>(this, callback);
        return Add(property.Name, call);
    }
    public ComponentBuilder<T> Set<TValue>(Expression<Func<T, EventCallback>> selector, Action<TValue> callback)
    {
        var property = Property(selector);
        var call = EventCallback.Factory.Create(this, callback);
        return Add(property.Name, call);
    }
    public ComponentBuilder<T> Set<TValue>(Expression<Func<T, EventCallback<TValue>>> selector, Action<TValue> callback)
    {
        var property = Property(selector);
        var call = EventCallback.Factory.Create(this, callback);
        return Add(property.Name, call);
    }


    public void CloseComponent(Action<T> action = null) => Build(action);

    /// <summary>
    /// 组件构建方法
    /// </summary>
    /// <param name="action">组件对象实例的委托</param>
    public void Build(Action<T> action = null)
    {
        builder.OpenComponent<T>(0); //开始附加组件
        if (Parameters.Count > 0)
            builder.AddMultipleAttributes(1, Parameters); //批量添加组件参数
        if (action != null)
            builder.AddComponentReferenceCapture(2, value => action.Invoke((T)value)); //返回组件对象实例
        builder.CloseComponent();   //结束附加组件
    }

    /// <summary>
    /// 通过属性选择器表达式获取组件参数属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private static PropertyInfo Property<TResult, TValue>(Expression<Func<TResult, TValue>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (selector.Body is not MemberExpression expression || expression.Member is not PropertyInfo propInfoCandidate)
            throw new ArgumentException($"The parameter selector '{selector}' does not resolve to a public property on the type '{typeof(T)}'.", nameof(selector));

        var type = typeof(T);
        var propertyInfo = propInfoCandidate.DeclaringType != type
                         ? type.GetProperty(propInfoCandidate.Name, propInfoCandidate.PropertyType)
                         : propInfoCandidate;
        return propertyInfo is null
            ? throw new ArgumentException($"The parameter selector '{selector}' does not resolve to a public property on the type '{typeof(T)}'.", nameof(selector))
            : propertyInfo;
    }
}