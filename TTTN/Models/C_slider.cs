namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_slider")]
    public partial class C_slider
    {
        [Key]
        public int slider_id { get; set; }

        [Required]
        [StringLength(100)]
        public string slider_name { get; set; }

        [Required]
        [StringLength(100)]
        public string slider_link { get; set; }

        [Required]
        [StringLength(50)]
        public string slider_position { get; set; }

        [Required]
        [StringLength(50)]
        public string slider_img { get; set; }

        public int slider_order { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime slider_createdat { get; set; }

        public int slider_createdby { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? slider_updatedat { get; set; }

        public int? slider_updatedby { get; set; }

        public int slider_status { get; set; }
    }
}
