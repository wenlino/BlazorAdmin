using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SqlSugar;

namespace AdminSenyun.Test.Entity.Wcc
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("PROADMIN")]
    public partial class PROADMIN
    {
        public PROADMIN()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsIdentity = true)]
        public int ID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int TYPE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>      
        [DisplayName("名称")]
        public string NAME { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("授权")]
        public string COMM { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("品项编码")]
        public string ARTICLENO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("员工")]
        public string EMPLOYEE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>    
        [DisplayName("创建日期")]
        public DateTime? DATECREATE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:True
        /// </summary>      
        [DisplayName("修改日期")]
        public DateTime? LCHANGE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string CUSTOMER { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("客户")]
        public string CLIENT { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("系列")]
        public string PROGRAM { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string CONTYPE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string DESIGN { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>     
        [DisplayName("颜色1")]
        public string COLOUR1 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("颜色2")]
        public string COLOUR2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("颜色3")]
        public string COLOUR3 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("颜色4")]
        public string COLOUR4 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("颜色5")]
        public string COLOUR5 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("信息1")]
        public string INFO1 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("信息2")]
        public string INFO2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("信息3")]
        public string INFO3 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("信息4")]
        public string INFO4 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("信息5")]
        public string INFO5 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("信息6")]
        public string INFO6 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string INFO7 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string INFO8 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string INFO9 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string INFO10 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string EDITOR { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LINE_NO { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int CNT { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("交货日期")]
        public string DELIVERY_DATE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_1 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_2 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_3 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_4 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_5 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_6 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_7 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_8 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_9 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string LEVEL_10 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string PICTURE_1 { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("简略文本")]
        public string TEXT_SHORT { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("描述")]
        public string TEXT_LONG { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int STATUS { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int PRODUCTIONID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("开始生产")]
        public string STARTDATE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("结束生产")]
        public string ENDDATE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string EXPENSE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        public string RESPONSE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int REFSTAT { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("账户")]
        public string SOURCE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int ORDERLOCK { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("发货日期")]
        public string SHIPPING_DATE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int EXPORTED { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public int CMS_PROCESS { get; set; }

        /// <summary>
        /// Desc:
        /// Default:1
        /// Nullable:False
        /// </summary>           
        public int CMS_CALCULATION { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public float CMS_PRICE { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int CMS_PRODUCTION { get; set; }

        /// <summary>
        /// Desc:
        /// Default:N''
        /// Nullable:False
        /// </summary>           
        [DisplayName("订单状态")]
        public string ORDERSTATUS { get; set; }
    }
}
