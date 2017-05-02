namespace LibraryWeb
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Library : DbContext
    {
        public Library(): base("name=Library")
        {
        }
              
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Author>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Author>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Author>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Author)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.BookName)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }
    }
}
