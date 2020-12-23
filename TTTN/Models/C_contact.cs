namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_contact")]
    public partial class C_contact
    {
        [Key]
        public int contact_id { get; set; }

        [Required]
        [StringLength(255)]
        public string contact_fullname { get; set; }

        [Required]
        [StringLength(255)]
        public string contact_email { get; set; }

        [Required]
        [StringLength(255)]
        public string contact_phone { get; set; }

        [Required]
        [StringLength(255)]
        public string contact_title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string contact_detail { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime contact_createdat { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? contact_updatedat { get; set; }

        public int? contact_updatedby { get; set; }

        public int contact_status { get; set; }
    }
}
