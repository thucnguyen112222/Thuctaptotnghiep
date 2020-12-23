namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_user")]
    public partial class C_user
    {
        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(200)]
        public string user_fullname { get; set; }

        [Required]
        [StringLength(200)]
        public string user_username { get; set; }

        [Required]
        [StringLength(64)]
        public string user_password { get; set; }

        [Required]
        public string user_email { get; set; }

        public int? user_gender { get; set; }

        [Required]
        [StringLength(11)]
        public string user_phone { get; set; }

        public string user_img { get; set; }

        public short user_access { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime user_createdat { get; set; }

        public int user_createdby { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? user_updatedat { get; set; }

        public int? user_updatedby { get; set; }

        public int user_status { get; set; }

        public string user_address { get; set; }
    }
}
