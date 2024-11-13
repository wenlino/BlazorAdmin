namespace AdminSenyun.Sys.Components;

/// <summary>
/// 
/// </summary>
public partial class Assignment
{
    private List<long> InternalValue
    {
        get { return Value; }
        set { Value = value; OnValueChanged(Value); }
    }
}
