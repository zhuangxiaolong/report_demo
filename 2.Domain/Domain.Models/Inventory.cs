using System;
using Domain.Core;

namespace Domain.Models
{
    public class Inventory : EntityBase<Inventory, long>
    {
        /// <summary>
        /// 箱码
        /// </summary>
        public long PackCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public int SapId { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        public string BatchId { get; set; }

        /// <summary>
        /// 扫描日期
        /// </summary>
        public DateTime? ScanDate { get; set; }

        /// <summary>
        /// 门店ID
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// 状态 -1假码 0未入库 1在库 2在架 3已售
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 销售次数
        /// </summary>
        public int? SellTimes { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductDate { get; set; }

        public int IsGift { get; set; }
        /// <summary>
        /// 禁止二次销售
        /// </summary>
        public int ProhibitTwoSales { get; set; }

        /// <summary>
        /// 发货区域
        /// </summary>
        public string BigArea { get; set; }
    }
}