using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;
using AdminSenyun.Data.Models;

namespace AdminSenyun.Data.Extensions;

static class SqlSugarHelper
{
    public static ConfigureExternalServices InitConfigureExternalServices()
    {
        return new ConfigureExternalServices()
        {
            EntityNameService = (type, entity) =>
            {
                //若是表名称不一致，更改
                if (type.GetCustomAttribute<TableAttribute>() is TableAttribute tableAttribute)
                {
                    entity.DbTableName = tableAttribute.Name;
                }
            },
            EntityService = (type, column) =>
            {
                //列名称改动 设置改动  
                if (type.GetCustomAttribute<KeyAttribute>() is KeyAttribute keyAttribute)
                {
                    //检查是否为key
                    column.IsPrimarykey = true;
                }

                if (type.GetCustomAttribute<DatabaseGeneratedAttribute>() is DatabaseGeneratedAttribute databaseGeneratedAttribute)
                {
                    //检查是否为自增列
                    column.IsIdentity = databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;
                }


                //名称
                if (type.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttribute)
                {
                    column.ColumnDescription = displayAttribute.Name ?? displayAttribute.Description;
                }

                //注释
                if (type.GetCustomAttribute<DescriptionAttribute>() is DescriptionAttribute descriptionAttribute)
                {
                    column.ColumnDescription = descriptionAttribute.Description;
                }


                //标记注释
                if (type.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttribute)
                {
                    column.ColumnDescription = displayNameAttribute.DisplayName;
                }

                //标签不存在列
                if (type.GetCustomAttribute<NotMappedAttribute>() is NotMappedAttribute)
                {
                    column.IsIgnore = true;
                }

                //名称不一致 ，更改
                if (type.GetCustomAttribute<ColumnAttribute>() is ColumnAttribute columnAttribute)
                {
                    column.DbColumnName = columnAttribute.Name;
                }


                //设置文字长度
                if (type.GetCustomAttribute<StringLengthAttribute>() is StringLengthAttribute lengthAttribute)
                {
                    column.Length = lengthAttribute.MaximumLength;
                }     

                //自动识别null
                if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                {
                    column.IsNullable = true;
                }

            }
        };
    }
}
