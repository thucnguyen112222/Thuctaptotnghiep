namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_category")]
    public partial class C_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C_category()
        {
            C_product = new HashSet<C_product>();
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C_product> C_product { get; set; }
        [Key]
        public int category_id { get; set; }
        [Required]
        [StringLength(255)]
        public string category_name { get; set; }
        [Required]
        [StringLength(255)]
        public string category_slug { get; set; }
        public int category_parentid { get; set; }
        public int category_order { get; set; }
        [Required]
        [StringLength(255)]
        public string category_metakey { get; set; }
        [Required]
        [StringLength(255)]
        public string category_metadesc { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime category_createdat { get; set; }
        public int category_createby { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? category_updatedat { get; set; }
        public int? category_updatedby { get; set; }
        public int category_status { get; set; }


    }
}
