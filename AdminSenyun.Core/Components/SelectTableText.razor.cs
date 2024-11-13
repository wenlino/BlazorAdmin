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
    /// ���/���� �Ƿ��Զ���������Ϣ Ĭ��Ϊ false
    /// </summary>
    [Parameter]
    public bool AutoGenerateColumns { get; set; } = true;

    /// <summary>
    /// ��ȡ�ı�ִ�з���
    /// </summary>
    [Parameter]
    public Func<TItem, string?>? GetTextCallback { get; set; }

    /// <summary>
    /// ���ݲ�ѯ����
    /// </summary>
    [Inject]
    [NotNull]
    private IDataService<TItem>? dataService { get; set; }

    /// <summary>
    /// ��ֵ
    /// </summary>
    [Parameter]
    public string? Value { get; set; }
    /// <summary>
    /// ֵ�ı��¼�
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }
    /// <summary>
    /// ֵ���ʽ
    /// </summary>
    [Parameter]
    public Expression<Func<string?>> ValueExpression { get; set; }

    /// <summary>
    /// �������ֶ�
    /// </summary>
    [Parameter]
    public Expression<Func<TItem, string>> FiledExpression { get; set; }

    /// <summary>
    /// �������ֶ�
    /// </summary>
    [Parameter]
    public Expression<Func<TItem, string>> TextExpression { get; set; }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        //��ʼ��
        search = new TItem();

        //�����ֶ���Ϣ
        if (ValueExpression != null)
        {
            _fieldIdentifier = FieldIdentifier.Create(ValueExpression);
        }

        if (FiledExpression != null)
        {
            //��ȡ���ֶ�
            var filedName = (FiledExpression.Body is MemberExpression m) ? m.Member.Name : "";

            //���ó�ʼ��ֵ
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
    /// ��ȡ ��ʾ����
    /// </summary>
    /// <returns></returns>
    public virtual string GetDisplayName()
    {
        return _fieldIdentifier?.GetDisplayName() ?? DisplayText ?? "";
    }


    /// <summary>
    /// ֵ�ı�ʱ��
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
    /// ��ѯ����
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private Task<QueryData<TItem>> OnQueryAsync(QueryPageOptions options)
    {
        return dataService.QueryAsync(options);
    }
}