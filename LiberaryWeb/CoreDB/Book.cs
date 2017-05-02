namespace LibraryWeb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Book")]
    public partial class Book
    {
        public int ID { get; set; }

        public int AuthorID { get; set; }

        [Required]
        [StringLength(50)]
        public string BookName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public virtual Author Author { get; set; }
        public string AuthorName { get; internal set; }
    }
}
