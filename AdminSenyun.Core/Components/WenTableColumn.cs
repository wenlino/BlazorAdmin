using AdminSenyun.Core.Attributes;
using AdminSenyun.Data.Core;
using AdminSenyun.Models;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Stimulsoft.Blockly.Model;
using System.Linq.Expressions;
using System.Reflection;

namespace AdminSenyun.Core.Components;

public class WenTableColumn<TItem, TType> : TableColumn<TItem, TType>
{
    [Inject, NotNull] private IDict? dict { get; set; }

    /// <summary>
    /// 绑定字段变化，主要作用智能提醒，不起其他作用
    /// </summary>
    [Parameter]
    public EventCallback<TType> FieldChanged { get; set; }

    private static List<SysDict> sysDicts;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (FieldExpression != null)
        {
            var body = FieldExpression.Body;

            if (body is MemberExpression member)
            {
                if (member.Member.GetCustomAttribute<DictAttribute>() is DictAttribute dictAttribute)
                {
                    sysDicts = dict.GetDicts(dictAttribute.Category);

                    Template ??= new RenderFragment<TableColumnContext<TItem, TType?>>(context => builder =>
                    {
                        var dict = sysDicts.FirstOrDefault(t => t.Code == context.Value?.ToString());

                        builder.OpenComponent<Tag>(0);
                        builder.AddComponentParameter(1, nameof(Tag.Color), dict?.Color ?? Color.None);
                        builder.AddComponentParameter(1, nameof(Tag.ChildContent),
                            new RenderFragment(bulder2 =>
                            {
                                bulder2.AddContent(0, dict?.Name ?? "");
                            }));
                        builder.CloseComponent();
                    });

                    EditTemplate ??= context => builder =>
                    {
                        var fieldName = GetFieldName();
                        var value = Utility.GetPropertyValue<object, TType?>(context, fieldName);

                        builder.OpenElement(0, "div");
                        builder.AddAttribute(1, "class", "col-12 col-sm-6");
                        builder.AddContent(2, builder2 =>
                        {
                            builder2.CloseElement();
                            builder2.OpenComponent<WenSelect<string>>(0);
                            builder2.AddComponentParameter(1, nameof(WenSelect<string>.DictCategory), dictAttribute.Category);
                            builder2.AddComponentParameter(2, nameof(WenSelect<string>.Value), value);
                            builder2.AddComponentParameter(2, nameof(WenSelect<string>.ValueChanged), EventCallback.Factory.Create<string>(this, text =>
                            {
                                Utility.SetPropertyValue(context, fieldName, text);
                            }));
                            builder2.CloseComponent();
                        });

                    };
                }
            }
        }
    }
}
