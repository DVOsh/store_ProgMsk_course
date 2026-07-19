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

        public async Task<Book[]> GetAllByIdsAsync(IEnumerable<int> bookIds)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dtos = await dbContext.Books
                                      .Where(book => bookIds.Contains(book.Id))
                                      .ToArrayAsync();


            return dtos.Select(Book.Mapper.Map)
                       .ToArray();
        }

        public async Task<Book[]> GetAllByIsbnAsync(string isbn)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            if (Book.TryFormatIsbn(isbn, out string formattedIsbn))
            {
                var dtoBooks = await dbContext.Books
                                              .Where(book => book.Isbn == formattedIsbn)
                                              .ToArrayAsync();

                return dtoBooks.Select(Book.Mapper.Map)
                               .ToArray();
            }

            return [];
        }

        public async Task<Book[]> GetAllByTitleOrAuthorAsync(string titleOrAuthor)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var parameter = new SqlParameter("@titleOrAuthor", titleOrAuthor);

            var dtoBooks = await dbContext.Books
                                          .FromSqlRaw("SELECT * FROM Books WHERE CONTAINS((Author, Title), @titleOrAuthor)",
                                                      parameter)
                                          .ToArrayAsync();

            return dtoBooks.Select(Book.Mapper.Map)
                           .ToArray();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dto = await dbContext.Books
                               .SingleAsync(book => book.Id == id);

            return Book.Mapper.Map(dto);
        }
    }
}
