namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_post")]
    public partial class C_post
    {
        [Key]
        public int post_id { get; set; }

        public int post_topid { get; set; }
      
        [Required]
        [Column(TypeName = "ntext")]
        public string post_title { get; set; }

        [Required]
        public string post_slug { get; set; }

        [Required]
        public string post_detail { get; set; }

        [Required]
        [StringLength(100)]
        public string post_type { get; set; }

        [Required]
        [StringLength(255)]
        public string post_metakey { get; set; }

        [Required]
        [StringLength(255)]
        public string post_metadesc { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime post_createdat { get; set; }

        public int post_createdby { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? post_updatedat { get; set; }

        public int? post_updatedby { get; set; }

        public int post_status { get; set; }
        [Required]
        [StringLength(255)]
        public string post_img { get; set; }
    }
}
