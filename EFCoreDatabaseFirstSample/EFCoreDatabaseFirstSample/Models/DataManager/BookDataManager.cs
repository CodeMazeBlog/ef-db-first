using System.Collections.Generic;
using System.Linq;
using EFCoreDatabaseFirstSample.Models.DTO;
using EFCoreDatabaseFirstSample.Models.Repository;

namespace EFCoreDatabaseFirstSample.Models.DataManager
{
    public class BookDataManager : IDataRepository<Book, BookDto>
    {
        readonly BookStoreContext _bookStoreContext;

        public BookDataManager(BookStoreContext storeContext)
        {
            _bookStoreContext = storeContext;
        }

        public IEnumerable<Book> GetAll()
        {
            throw new System.NotImplementedException();
        }
        
        public Book Get(long id)
        {
            _bookStoreContext.ChangeTracker.LazyLoadingEnabled = false;

            var book = _bookStoreContext.Book
                .SingleOrDefault(b => b.Id == id);

            if (book == null)
            {
                return null;
            }

            _bookStoreContext.Entry(book)
                .Collection(b => b.BookAuthors)
                .Load();

            _bookStoreContext.Entry(book)
                .Reference(b => b.Publisher)
                .Load();
            
            return book;
        }

        public BookDto GetDto(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Add(Book entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Book entityToUpdate, Book entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Book entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
