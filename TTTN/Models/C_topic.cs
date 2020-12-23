namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_topic")]
    public partial class C_topic
    {
        [Key]
        public int topic_id { get; set; }

        [Required]
        [StringLength(255)]
        public string topic_name { get; set; }

        [Required]
        [StringLength(255)]
        public string topic_slug { get; set; }

        public int topic_parentid { get; set; }

        public int topic_order { get; set; }

        [Required]
        [StringLength(255)]
        public string topic_metakey { get; set; }

        [Required]
        [StringLength(255)]
        public string topic_metadesc { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime topic_createdat { get; set; }

        public int topic_createdby { get; set; }

        public int topic_updatedby { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? topic_updatedat { get; set; }

        public int topic_status { get; set; }
    }
}
