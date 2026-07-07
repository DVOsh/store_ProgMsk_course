namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books =
        [
            new Book(1,
                "ISBN 9780201896831",
                "Donald E. Knuth", 
                "The Art of Computer Programming, Vol. 1",
                "This first volume in the series begins with basic programming concepts and techniques, then focuses more particularly on information structures–the representation of information inside a computer, the structural relationships between data elements and how to deal with them efficiently. Elementary applications are given to simulation, numerical methods, symbolic computing, software and system design. Dozens of simple and important algorithms and techniques have been added to those of the previous edition. The section on mathematical preliminaries has been extensively revised to match present trends in research.",
                57m),
            new Book(2,
                "ISBN 0134757599",
                "Martin Fowler",
                "Refactoring: Improving the Design of Existing Code",
                "This eagerly awaited new edition has been fully updated to reflect crucial changes in the programming landscape. Refactoring, Second Edition, features an updated catalog of refactoring's and includes JavaScript code examples, as well as new functional examples that demonstrate refactoring without classes.",
                46.12m),
            new Book(3,
                "ISBN 0131103628",
                "Brian W. Kernighan, Dennis M. Ritchie", 
                "C Programming Language",
                "The authors present the complete guide to ANSI standard C language programming. Written by the developers of C, this new version helps readers keep up with the finalized ANSI standard for C while showing how to take advantage of C's rich set of operators, economy of expression, improved control flow, and data structures. The 2/E has been completely rewritten with additional examples and problem sets to clarify the implementation of difficult language constructs. For years, C programmers have let K&R guide them to building well-structured and efficient programs. Now this same help is available to those working with ANSI compilers. Includes detailed coverage of the C language plus the official C language reference manual for at-a-glance help with syntax notation, declarations, ANSI changes, scope rules, and the list goes on and on.",
                34.36m),
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

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
