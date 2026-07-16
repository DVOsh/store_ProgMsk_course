using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF
{
    public class BookRepository : IBookRepository
    {
        private readonly DbContextFactory dbContextFactory;
        //private readonly StoreDbContext storeDbContext;

        //todo: добавить инициализацию dbContextFactory в конструкторе

        public BookRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            //storeDbContext = dbContextFactory.Create(typeof(BookRepository));
        }

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            return dbContext.Books
                            .Where(book => bookIds.Contains(book.Id))
                            .AsEnumerable()
                            .Select(Book.Mapper.Map)
                            .ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            if (Book.TryFormatIsbn(isbn, out string formattedIsbn))
            {
                return dbContext.Books
                                .Where(book => book.Isbn == formattedIsbn)
                                .Select(Book.Mapper.Map)
                                .AsEnumerable()
                                .ToArray();
            }

            return [];
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var parameter = new SqlParameter("@titleOrAuthor", titleOrAuthor);
            return dbContext.Books
                            .FromSqlRaw("SELECT * FROM Books WHERE CONTAINS((Author, Title), @titleOrAuthor)", 
                                        parameter)
                            .AsEnumerable()
                            .Select(Book.Mapper.Map)
                            .ToArray();
        }

        public Book GetById(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dto = dbContext.Books
                               .Single(book => book.Id == id);

            return Book.Mapper.Map(dto);
        }
    }
}
