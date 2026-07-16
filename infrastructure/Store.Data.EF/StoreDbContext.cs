using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<BookDto> Books { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBooks(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
        }

        private static void BuildBooks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDto>(action =>
            {
                action.Property(dto => dto.Isbn)
                      .HasMaxLength(17)
                      .IsRequired();

                action.Property(dto => dto.Title)
                      .IsRequired();

                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasData(
                    new BookDto
                    {
                        Id = 1,
                        Isbn = "ISBN9780201896831",
                        Author = "Donald E. Knuth",
                        Title = "The Art of Computer Programming, Vol. 1",
                        Description = "This first volume in the series begins with basic programming concepts and techniques, then focuses more particularly on information structures–the representation of information inside a computer, the structural relationships between data elements and how to deal with them efficiently. Elementary applications are given to simulation, numerical methods, symbolic computing, software and system design. Dozens of simple and important algorithms and techniques have been added to those of the previous edition. The section on mathematical preliminaries has been extensively revised to match present trends in research.",
                        Price = 57m
                    },
                    new BookDto
                    {
                        Id = 2,
                        Isbn = "ISBN0134757599",
                        Author = "Martin Fowler",
                        Title = "Refactoring: Improving the Design of Existing Code",
                        Description = "This eagerly awaited new edition has been fully updated to reflect crucial changes in the programming landscape. Refactoring, Second Edition, features an updated catalog of refactoring's and includes JavaScript code examples, as well as new functional examples that demonstrate refactoring without classes.",
                        Price = 16.12m
                    },
                    new BookDto
                    {
                        Id = 3,
                        Isbn = "ISBN0131103628",
                        Author = "Brian W. Kernighan, Dennis M. Ritchie",
                        Title = "C Programming Language",
                        Description = "The authors present the complete guide to ANSI standard C language programming. Written by the developers of C, this new version helps readers keep up with the finalized ANSI standard for C while showing how to take advantage of C's rich set of operators, economy of expression, improved control flow, and data structures. The 2/E has been completely rewritten with additional examples and problem sets to clarify the implementation of difficult language constructs. For years, C programmers have let K&R guide them to building well-structured and efficient programs. Now this same help is available to those working with ANSI compilers. Includes detailed coverage of the C language plus the official C language reference manual for at-a-glance help with syntax notation, declarations, ANSI changes, scope rules, and the list goes on and on.",
                        Price = 34.36m
                    });
            });
        }

        private void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.CellPhone)
                      .HasMaxLength(20);

                action.Property(dto => dto.DeliveryUniqueCode)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryPrice)
                      .HasColumnType("money");

                action.Property(dto => dto.DeliveryParameters)
                      .HasConversion(
                            value => JsonConvert.SerializeObject(value),
                            value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.PaymentServiceName)
                      .HasMaxLength(40);

                action.Property(dto => dto.PaymentParameters)
                      .HasConversion(
                            value => JsonConvert.SerializeObject(value),
                            value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);
            });
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasOne(dto => dto.Order)
                      .WithMany(dto => dto.Items)
                      .IsRequired();
            });
        }

        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>(
                    (dictionary1, dictionary2) =>
                        dictionary1.SequenceEqual(dictionary2),
                    dictionary => dictionary.Aggregate(
                        0,
                        (a, p) => HashCode.Combine(
                            HashCode.Combine(a, p.Key.GetHashCode(), p.Value.GetHashCode())
                        )
                    )
            );
    }
}
