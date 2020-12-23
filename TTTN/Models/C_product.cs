namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_product")]
    public partial class C_product
    {
        [Key]
        public int product_id { get; set; }

        public int product_catid { get; set; }

        [Required]
        [StringLength(255)]
        public string product_name { get; set; }

        [Required]
        [StringLength(255)]
        public string product_slug { get; set; }

        [Required]
        [StringLength(100)]
        public string product_img { get; set; }

        [Required]
        public string product_detail { get; set; }

        public int product_number { get; set; }

        public int product_price { get; set; }

        public int? product_pricesale { get; set; }

        [Required]
        [StringLength(255)]
        public string product_metakey { get; set; }

        [Required]
        [StringLength(255)]
        public string product_metadesc { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime product_createdat { get; set; }

        public int product_createdby { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? product_updatedat { get; set; }

        public int? product_updatedby { get; set; }

        public int product_status { get; set; }
        public int? product_importprice { get; set; }
        public virtual C_category C_category { get; set; }
    }
}
