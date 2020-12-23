namespace TTTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("_link")]
    public partial class C_link
    {
        public int id { get; set; }

        public string slug { get; set; }

        public int? tableid { get; set; }

        [StringLength(50)]
        public string type { get; set; }
    }
}
