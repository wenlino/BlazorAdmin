using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace AdminSenyun.Core.Components;

public partial class SelectTableText<TItem> where TItem : class, new()
{
    private TItem? search { get; set; }

    private FieldIdentifier? _fieldIdentifier;

    private string? DisplayText { get; set; }

    /// <summary>
    /// 获得/设置 是否自动生成列信息 默认为 false
    /// </summary>
    [Parameter]
    public bool AutoGenerateColumns { get; set; } = true;

    /// <summary>
    /// 获取文本执行方法
    /// </summary>
    [Parameter]
    public Func<TItem, string?>? GetTextCallback { get; set; }

    /// <summary>
    /// 数据查询服务
    /// </summary>
    [Inject]
    [NotNull]
    private IDataService<TItem>? dataService { get; set; }

    /// <summary>
    /// 绑定值
    /// </summary>
    [Parameter]
    public string? Value { get; set; }
    /// <summary>
    /// 值改变事件
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }
    /// <summary>
    /// 值表达式
    /// </summary>
    [Parameter]
    public Expression<Func<string?>> ValueExpression { get; set; }

    /// <summary>
    /// 绑定内容字段
    /// </summary>
    [Parameter]
    public Expression<Func<TItem, string>> FiledExpression { get; set; }

    /// <summary>
    /// 绑定名称字段
    /// </summary>
    [Parameter]
    public Expression<Func<TItem, string>> TextExpression { get; set; }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        //初始化
        search = new TItem();

        //设置字段信息
        if (ValueExpression != null)
        {
            _fieldIdentifier = FieldIdentifier.Create(ValueExpression);
        }

        if (FiledExpression != null)
        {
            //获取绑定字段
            var filedName = (FiledExpression.Body is MemberExpression m) ? m.Member.Name : "";

            //设置初始化值
            search.GetType().GetProperty(filedName).SetValue(search, Value);


            if (GetTextCallback is null)
            {
                if (TextExpression != null)
                {
                    GetTextCallback = TextExpression.Compile();
                }
                else
                {
                    GetTextCallback = FiledExpression.Compile();
                }
            }
        }
    }

    /// <summary>
    /// 获取 显示名称
    /// </summary>
    /// <returns></returns>
    public virtual string GetDisplayName()
    {
        return _fieldIdentifier?.GetDisplayName() ?? DisplayText ?? "";
    }


    /// <summary>
    /// 值改变时间
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public async Task OnValueChanged(TItem val)
    {
        var text = FiledExpression.Compile().Invoke(val);
        if (ValueChanged.HasDelegate)
            ValueChanged.InvokeAsync(text);
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private Task<QueryData<TItem>> OnQueryAsync(QueryPageOptions options)
    {
        return dataService.QueryAsync(options);
    }
}