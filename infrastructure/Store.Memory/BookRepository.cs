namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books =
        [
            new Book(1, "ISBN 12312-31231", "D. Knuth", "Art Of Programming"),
            new Book(2, "ISBN 13212-32121", "M. Fowler", "Refactoring"),
            new Book(3, "ISBN 32131-13212", "B. Kernighan, D. Ritchie", "C Programming Language"),
        ];

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn)
                .ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titlePart)
        {
            return books.Where(book => book.Author.Contains(titlePart) 
                                    || book.Title.Contains(titlePart))
                        .ToArray();
        }
    }
}
