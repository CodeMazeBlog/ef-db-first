using Microsoft.EntityFrameworkCore;

namespace EFCoreDatabaseFirstSample.Models
{
    public partial class BookStoreContext : DbContext
    {
        public BookStoreContext()
        {
        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<AuthorContact> AuthorContact { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<BookAuthors> BookAuthors { get; set; }
        public virtual DbSet<BookCategory> BookCategory { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer("Server=.;Database=BookStore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AuthorContact>(entity =>
            {
                entity.HasKey(e => e.AuthorId)
                    .HasName("PK__AuthorCo__70DAFC34A95F26A8");

                entity.Property(e => e.AuthorId).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.ContactNumber).HasMaxLength(15);

                entity.HasOne(d => d.Author)
                    .WithOne(p => p.AuthorContact)
                    .HasForeignKey<AuthorContact>(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuthorCon__Autho__398D8EEE");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Book)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Book__CategoryId__403A8C7D");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Book__PublisherI__412EB0B6");
            });

            modelBuilder.Entity<BookAuthors>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.AuthorId })
                    .HasName("PK__BookAuth__6AED6DC4F50D1BAA");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookAutho__Autho__44FF419A");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BookAutho__BookI__440B1D61");
            });

            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
