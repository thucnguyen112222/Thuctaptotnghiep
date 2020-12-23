namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_orderdetail")]
    public partial class C_orderdetail
    {
        [Key]
        public int orderdetail_id { get; set; }

        public int orderdetail_orderid { get; set; }

        public int orderdetail_productid { get; set; }

        public int orderdetail_price { get; set; }

        public int orderdetail_quanity { get; set; }

        public int orderdetail_amount { get; set; }

        [StringLength(255)]
        public string orderdetail_img { get; set; }

        [StringLength(255)]
        public string orderdetail_name { get; set; }

        public int? orderdetail_total { get; set; }

        [StringLength(12)]
        public string orderdetail_code { get; set; }

        public virtual C_order C_order { get; set; }
    }
}
