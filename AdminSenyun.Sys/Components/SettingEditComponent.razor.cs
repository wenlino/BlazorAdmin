using System.ComponentModel;

namespace AdminSenyun.Sys.Components;

public partial class SettingEditComponent
{
    [CascadingParameter(Name = "BodyContext")]
    private object BodyContext { get; set; }

    [CascadingParameter]
    [NotNull]
    private Modal Dialog { get; set; }

    [Inject]
    private ISysSetting sysSettingService { get; set; }


    private string isPassword => sysSetting.IsHide ? "password" : "";
    private SysSetting sysSetting { get; set; } = new SysSetting();

    [Description("内容/值")]
    public long sysSettingNumberValue
    {
        get => long.TryParse(sysSetting.Value, out long result) ? result : 0;
        set => sysSetting.Value = value.ToString();
    }

    private IEnumerable<SelectedItem> Items { get; set; } = [new SelectedItem("是", "是"), new SelectedItem("否", "否")];
    private IEnumerable<SelectedItem> GetSelectedItems() => sysSetting?.Items.Split(',').Select(t => new SelectedItem(t, t)).ToList();

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (BodyContext is SysSetting sysSetting)
        {
            this.sysSetting = sysSetting;
            if (sysSetting.Typ == SettingTyp.SqlServer)
                serverTag.SetConnection(sysSetting.Value);
            if (sysSetting.Typ == SettingTyp.Access)
            {
                if (sysSetting.Value.Contains("Data Source="))
                {
                    Value = sysSetting.Value[(sysSetting.Value.IndexOf("Data Source=") + "Data Source=".Length)..];
                }
            }
            else if (sysSetting.Typ == SettingTyp.Sqlite)
            {
                if (sysSetting.Value.Contains("DataSource="))
                {
                    Value = sysSetting.Value[(sysSetting.Value.IndexOf("DataSource=") + "DataSource=".Length)..];
                }
            }
        }
    }

    private Task ServerTagValueChange()
    {
        sysSetting.Value = serverTag.GetConnectionString();
        StateHasChanged();
        return Task.FromResult(true);
    }

    private Task SysSettingValueChange()
    {
        serverTag.SetConnection(sysSetting.Value);
        StateHasChanged();
        return Task.FromResult(true);
    }

    private async Task EditSaveAsync()
    {
        sysSettingService.SaveSetting(sysSetting, false);
        await Dialog.Close();
        await ToastService.Success("保存成功", $"{sysSetting.Title}:{sysSetting.Value}");
    }

    [Description("数据参数")]
    public string Value { get; set; } = "";
    private Task ServerAccessValueChange()
    {
        sysSetting.Value = "Provider=Microsoft.ACE.OleDB.12.0;Data Source=" + Value;
        //sysSetting.Value = "Provider=Microsoft.ACE.OleDB.15.0;Data Source=" + Value;
        return Task.FromResult(true);
    }

    private Task ServerSqliteValueChange()
    {
        sysSetting.Value = "DataSource=" + Value;
        return Task.FromResult(true);
    }

    public ServerTag serverTag { get; set; } = new ServerTag();

    public class ServerTag
    {
        [DisplayName("服务器地址")]
        public string Server { get; set; } = "";
        [DisplayName("数据库名称")]
        public string Database { get; set; } = "";
        [DisplayName("登录用户名")]
        public string UserId { get; set; } = "";
        [DisplayName("登录密码")]
        public string Password { get; set; } = "";

        public void SetConnection(string connectionString)
        {
            try
            {
                var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                Server = builder.DataSource;
                Database = builder.InitialCatalog;
                UserId = builder.UserID;
                Password = builder.Password;
            }
            catch { }
        }

        public string GetConnectionString()
        => $"Data Source={Server};Initial Catalog={Database};User ID={UserId};Password={Password};Persist Security Info=True;TrustServerCertificate=True";

        public override string ToString()
        {
            return $"Data Source={Server};Initial Catalog={Database};User ID={UserId};Password={Password};Persist Security Info=True;TrustServerCertificate=True";
        }
    }
}