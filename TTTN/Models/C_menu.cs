namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_menu")]
    public partial class C_menu
    {
        [Key]
        public int menu_id { get; set; }

        [Required]
        [StringLength(255)]
        public string menu_name { get; set; }

        [Required]
        [StringLength(255)]
        public string menu_link { get; set; }

        [Required]
        [StringLength(50)]
        public string menu_type { get; set; }

        public int menu_tableid { get; set; }

        public int menu_order { get; set; }

        [Required]
        [StringLength(255)]
        public string menu_position { get; set; }

        public int menu_parentid { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime menu_createdat { get; set; }

        public int menu_createdby { get; set; }

        public int menu_status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? menu_updatedat { get; set; }

        public int? menu_updatedby { get; set; }
    }
}
