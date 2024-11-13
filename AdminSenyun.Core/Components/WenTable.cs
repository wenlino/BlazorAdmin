using AdminSenyun.Core.Extensions;
using AdminSenyun.Data.Core;
using AdminSenyun.Models;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using SqlSugar;
using System.Text.Json;

namespace AdminSenyun.Core.Components
{
    [CascadingTypeParameter("TItem")]
    [JSModuleAutoLoader("./_content/BootstrapBlazor/Components/Table/Table.razor.js", JSObjectReference = true)]
    public class WenTable<TItem> : Table<TItem> where TItem : class, new()
    {
        public WenTable()
        {
            //this.IsStriped = true;
            this.IsBordered = true;
            this.IsMultipleSelect = true;
            this.IsPagination = true;
            this.IsFixedHeader = true;
            this.ShowToolbar = true;
            this.ShowEmpty = true;
            this.ShowLoading = true;
            this.ShowSearch = true;
            this.ShowAdvancedSearch = true;
            this.ShowDefaultButtons = true;

            this.ShowExtendButtons = true;

            this.FixedExtendButtonsColumn = true;
            this.ClickToSelect = true;
            this.ShowColumnList = true;
            this.IsFixedFooter = true;
            this.ShowSkeleton = true;
            this.ShowCardView = true;
            this.ShowLineNo = true;

            this.ShowEmpty = true;
            this.EmptyText = "暂无数据";
            this.EmptyImage = "images/empty.svg";


            //列数据持久化解决方案
            this.AllowDragColumn = true;
            this.AllowResizing = true;
            base.OnDragColumnEndAsync = DragColumnEndAsync;
            base.OnResizeColumnAsync = ResizeColumnAsync;
            base.OnColumnVisibleChanged = ColumnVisibleChanged;
            base.OnColumnCreating = ColumnCreating;

            base.SelectedRowsChanged =
                EventCallback.Factory.Create(this, (List<TItem> items) => OnSelectedRowsChanged(items));
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            //数据导出按钮
            this.ShowExportButton = IsExport;
            this.ShowExportCsvButton = IsExport;
            this.ShowExportExcelButton = IsExport;

            //是否显示新增按钮
            this.ShowAddButton = IsAddButton ?? AuthorizeButton("add");
            //this.ShowAddButton = false;

            //是否显示编辑按钮
            this.ShowEditButton = IsEditButton ?? AuthorizeButton("edit");
            this.ShowExtendEditButton = IsEditButton ?? AuthorizeButton("edit");

            //是否显示删除按钮
            this.ShowDeleteButton = IsDeleteButton ?? AuthorizeButton("del");
            this.ShowExtendDeleteButton = IsDeleteButton ?? AuthorizeButton("del");


            //设置数据库
            this.SqlSugarClient ??= string.IsNullOrWhiteSpace(SysSettingSqlName) ? db : sysSetting.GetSqlSugarClient(SysSettingSqlName);
            //默认数据服务
            this.OnQueryAsync ??= QueryAsync;
            this.OnSaveAsync ??= SaveAsync;
            this.OnDeleteAsync ??= DeleteAsync;

            base.CreateItemCallback ??= OnCreateItemCallback;


            ////新增按钮
            //RenderFragment addButton = builder =>
            //{
            //    builder.Component<TableToolbarButton<TItem>>()
            //    .Set(t => t.Color, Color.Success)
            //    .Set(t => t.OnClick, EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, AddAsync))
            //    .Set(t => t.IsDisabled, DisableAddButtonCallback?.Invoke() ?? false)
            //    .Set(t => t.Icon, AddButtonIcon)
            //    .Set(t => t.Text, AddButtonText)
            //    .Build();
            //};

            //TableToolbarBeforeTemplate = builder =>
            //{
            //    if (IsAddButton ?? AuthorizeButton("add"))
            //    {
            //        addButton(builder);
            //    }
            //};
        }


        #region 注入服务
        /// <summary>
        /// 数据库服务
        /// </summary>
        [NotNull]
        [Inject]
        private ISqlSugarClient? db { get; set; }

        /// <summary>
        /// 缓存服务
        /// </summary>
        [NotNull]
        [Inject]
        private ICache? cache { get; set; }

        /// <summary>
        /// 注入地址管理器服务
        /// </summary>
        [Inject]
        [NotNull]
        private NavigationManager? NavigationManager { get; set; }

        /// <summary>
        /// 注入管理服务
        /// </summary>
        [Inject]
        [NotNull]
        private IAdmin? AdminService { get; set; }


        [Inject]
        [NotNull]
        private ISysSetting? sysSetting { get; set; }

        #endregion


        #region 参数
        /// <summary>
        /// 是否显示添加按钮 默认读取配置权限
        /// </summary>
        [Parameter]
        public bool? IsAddButton { get; set; }

        /// <summary>
        /// 是否显示编辑按钮 默认读取配置权限
        /// </summary>
        [Parameter]
        public bool? IsEditButton { get; set; }

        /// <summary>
        /// 是否显示删除按钮 默认读取配置权限
        /// </summary>
        [Parameter]
        public bool? IsDeleteButton { get; set; }

        /// <summary>
        /// 服务器 列数据持久化存储解决方案  填写名称即可，多个表不要重复，服务器会覆盖 自动按照用户存储
        /// </summary>
        [Parameter]
        public string? ColumnPersistenceStoreName { get; set; }

        /// <summary>
        /// 是否单选
        /// </summary>
        [Parameter]
        public bool IsRadio { get; set; }

        /// <summary>
        /// 是否使用导出按钮
        /// </summary>
        [Parameter]
        public bool IsExport { get; set; } = true;

        /// <summary>
        /// 获得/设置 选中行变化方法
        /// </summary>
        [Parameter]
        public Func<List<TItem>, Task>? OnSelectRowsAsync { get; set; }

        /// <summary>
        /// 数据链接 默认系统注入ISqlSugarClient
        /// </summary>
        [NotNull]
        [Parameter]
        public ISqlSugarClient? SqlSugarClient { get; set; }

        /// <summary>
        /// 从设置库中读取 设置设置库中的Name名称
        /// </summary>
        [Parameter]
        public string? SysSettingSqlName { get; set; }

        /// <summary>
        /// 默认是否按照创建时间倒序
        /// </summary>
        [Parameter]
        public bool? IsCreateTimeDesc { get; set; }

        ///// <summary>
        ///// 新增按钮禁用回调
        ///// </summary>
        //[Parameter]
        //public Func<bool>? DisableAddButtonCallback { get; set; }
        #endregion


        #region 选中事件

        /// <summary>
        /// 选中行变化事件
        /// </summary>
        /// <param name="items"></param>
        public void OnSelectedRowsChanged(List<TItem> items)
        {
            if (IsRadio == true && items.Count > 0)
            {
                items = [items.Last()];
                SelectedRows = items;
            }
            OnSelectRowsAsync?.Invoke(items);
        }

        #endregion


        #region 按钮权限
        /// <summary>
        /// 设置按钮权限
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        private bool AuthorizeButton(string operate)
        {
            var url = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            return AdminService.AuthorizingBlock(AdminService.UserName, url, operate);
        }
        #endregion


        #region 列改动数据持久化存储方案

        /// <summary>
        /// 位置拖拽事件
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private async Task DragColumnEndAsync(string columnName, IEnumerable<ITableColumn> columns)
        {
            await ColumnChangedPersistenceStore(columns);
        }

        /// <summary>
        /// 列宽度改变事件
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private async Task ResizeColumnAsync(string columnName, float width)
        {
            var col = Columns.FirstOrDefault(t => t.GetFieldName() == columnName);
            if (col == null) return;
            col.Width = (int)width;
            await ColumnChangedPersistenceStore(Columns);
        }

        /// <summary>
        /// 列改变隐藏/显示状态回调方法
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        private async Task ColumnVisibleChanged(string columnName, bool visible)
        {
            var col = Columns.FirstOrDefault(t => t.GetFieldName() == columnName);
            if (col == null) return;
            col.Visible = visible;
            await ColumnChangedPersistenceStore(Columns);
        }

        class PersistenceStoreColumn
        {
            public string Name { get; set; }
            public string? Text { get; set; }
            public bool? Visible { get; set; }
            public int? Width { get; set; }
            public int Order { get; set; }
        }

        static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// 客户数据持久化存储在服务器中，自动按照用户存储
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private async Task ColumnChangedPersistenceStore(IEnumerable<ITableColumn> columns)
        {
            if (string.IsNullOrWhiteSpace(ColumnPersistenceStoreName)) return;

            var lis = new List<object>();

            int index = 1;
            foreach (var item in columns)
            {
                var li = new PersistenceStoreColumn()
                {
                    Name = item.GetFieldName(),
                    Text = item.Text,
                    Visible = item.Visible,
                    Width = item.Width,
                    Order = index++
                };
                lis.Add(li);
            }
            var col = JsonSerializer.Serialize(lis, jsonSerializerOptions);

            db.Storageable(new SysUserLocalStorageData()
            {
                TableName = ColumnPersistenceStoreName,
                UserName = AdminService.UserName,
                Value = col
            }).WhereColumns(t => new { t.TableName, t.UserName }).ExecuteCommand();
            cache.Remove<SysUserLocalStorageData>();
        }


        /// <summary>
        /// 列创建时回调事件
        /// </summary>
        /// <param name="tableColumns"></param>
        /// <returns></returns>
        private async Task ColumnCreating(List<ITableColumn> tableColumns)
        {
            //是否显示搜索
            var showSearch = tableColumns.Exists(t => t.Searchable == true);
            //设置搜索功能是否显示
            this.ShowSearch = showSearch;
            this.ShowSearchButton = showSearch;
            this.ShowAdvancedSearch = showSearch;

            if (string.IsNullOrWhiteSpace(ColumnPersistenceStoreName)) return;
            try
            {
                var storageData =
                    cache.GetAll<SysUserLocalStorageData>()
                    .Where(t => t.TableName == ColumnPersistenceStoreName && t.UserName == AdminService.UserName)
                    .FirstOrDefault();

                if (storageData == null || string.IsNullOrWhiteSpace(storageData?.Value)) return;

                var cols = JsonDocument.Parse(storageData.Value).Deserialize<List<PersistenceStoreColumn>>();

                if (cols == null || cols.Count == 0) return;

                var columns = new List<ITableColumn>();
                foreach (var item in tableColumns)
                {
                    var col = cols.FirstOrDefault(it => it.Name == item.GetFieldName() && it.Text == item.Text);
                    if (col != null)
                    {
                        item.Order = col.Order;
                        item.Width = col.Width;
                        item.Visible = col.Visible;
                    }
                    columns.Add(item);
                }
                Columns.Clear();
                Columns.AddRange(columns.OrderBy(it => it.Order));
            }
            catch { }
        }

        #endregion


        #region 数据服务

        Task<QueryData<TItem>> QueryAsync(QueryPageOptions option)
        {
            int count = 0;
            var filter = option.ToFilter();
            var items = SqlSugarClient.Queryable<TItem>()
                          .WhereIF(filter.HasFilters(), filter.GetFilterLambda<TItem>())
                          .OrderByPropertyNameIF(option.SortOrder != SortOrder.Unset, option.SortName,
                          option.SortOrder == SortOrder.Asc ? OrderByType.Asc : OrderByType.Desc)
                          .OrderByPropertyNameIF(typeof(IEntityBase).IsAssignableFrom(typeof(TItem)) && (IsCreateTimeDesc ?? false), "CreateTime", OrderByType.Desc)
                          .ToPageList(option.PageIndex, option.PageItems, ref count);
            var data = new QueryData<TItem>
            {
                IsSorted = option.SortOrder != SortOrder.Unset,
                IsFiltered = option.Filters.Any(),
                IsAdvanceSearch = option.AdvanceSearches.Any(),
                IsSearch = option.Searches.Any() || option.CustomerSearches.Any(),
                Items = items,
                TotalCount = Convert.ToInt32(count)
            };
            return Task.FromResult(data);
        }


        Task<bool> SaveAsync(TItem model, ItemChangedType changedType)
        {
            try
            {
                if (changedType == ItemChangedType.Add)
                {
                    SqlSugarClient.Insertable(model).ExecuteCommand();
                }
                else if (changedType == ItemChangedType.Update)
                {
                    SqlSugarClient.Updateable(model).ExecuteCommand();
                }
                return Task.FromResult(true);
            }
            catch { return Task.FromResult(false); }
        }

        Task<bool> DeleteAsync(IEnumerable<TItem> models)
        {
            try
            {
                SqlSugarClient.Deleteable<TItem>(models).ExecuteCommand();
                return Task.FromResult(true);
            }
            catch { return Task.FromResult(false); }
        }

        #endregion


        #region 拓展方法 复制新增

        private TItem? addModel;
        private TItem OnCreateItemCallback()
        {
            if (addModel is null)
            {
                try
                {
                    return Activator.CreateInstance<TItem>();
                }
                catch
                {
                    throw new Exception("未提供无参构造函数 new()");
                }
            }
            else
            {
                return addModel;
            }
        }

        /// <summary>
        /// 调用新增防范
        /// </summary>
        /// <param name="item">传递元素model</param>
        /// <returns></returns>
        public async Task AddAsync(TItem item)
        {
            this.addModel = item;
            await base.AddAsync();
            this.addModel = null;
        }
        #endregion


    }
}
