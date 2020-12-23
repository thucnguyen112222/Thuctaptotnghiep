namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_order")]
    public partial class C_order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public C_order()
        {
            C_orderdetail = new HashSet<C_orderdetail>();
        }

        [Key]
        public int order_id { get; set; }

        [Required]
        [StringLength(12)]
        public string order_code { get; set; }

        public int order_userid { get; set; }

        public DateTime order_createdate { get; set; }

        public DateTime? order_exportdate { get; set; }

        [Required]
        public string order_deliveryaddress { get; set; }

        [Required]
        [StringLength(100)]
        public string order_deliveryname { get; set; }

        [Required]
        [StringLength(20)]
        public string order_deliveryphone { get; set; }

        [Required]
        [StringLength(50)]
        public string order_email { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? order_updatedat { get; set; }

        public int? order_updatedby { get; set; }

        public int order_createdby { get; set; }

        public int order_status { get; set; }

        [Required]
        [StringLength(100)]
        public string order_name { get; set; }

        public int? order_payment { get; set; }

        public DateTime? order_order { get; set; }

        public DateTime? order_deliverydate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<C_orderdetail> C_orderdetail { get; set; }
    }
}
