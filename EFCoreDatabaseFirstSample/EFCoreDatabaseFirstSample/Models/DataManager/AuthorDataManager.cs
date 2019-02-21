using System.Collections.Generic;
using System.Linq;
using EFCoreDatabaseFirstSample.Models.DTO;
using EFCoreDatabaseFirstSample.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDatabaseFirstSample.Models.DataManager
{
    public class AuthorDataManager : IDataRepository<Author, AuthorDto>
    {
        readonly BookStoreContext _bookStoreContext;

        public AuthorDataManager(BookStoreContext storeContext)
        {
            _bookStoreContext = storeContext;
        }

        public IEnumerable<Author> GetAll()
        {
            return _bookStoreContext.Author
                .Include(author => author.AuthorContact)
                .ToList();
        }

        public Author Get(long id)
        {
            var author = _bookStoreContext.Author
                .SingleOrDefault(b => b.Id == id);

            return author;
        }

        public AuthorDto GetDto(long id)
        {
            _bookStoreContext.ChangeTracker.LazyLoadingEnabled = true;

            using (var context = new BookStoreContext())
            {
                var author = context.Author
                    .SingleOrDefault(b => b.Id == id);

                return AuthorDtoMapper.MapToDto(author);
            }
        }


        public void Add(Author entity)
        {
            _bookStoreContext.Author.Add(entity);
            _bookStoreContext.SaveChanges();
        }

        public void Update(Author entityToUpdate, Author entity)
        {
            entityToUpdate = _bookStoreContext.Author
                .Include(a => a.BookAuthors)
                .Include(a => a.AuthorContact)
                .Single(b => b.Id == entityToUpdate.Id);

            entityToUpdate.Name = entity.Name;

            entityToUpdate.AuthorContact.Address = entity.AuthorContact.Address;
            entityToUpdate.AuthorContact.ContactNumber = entity.AuthorContact.ContactNumber;

            var deletedBooks = entityToUpdate.BookAuthors.Except(entity.BookAuthors).ToList();
            var addedBooks = entity.BookAuthors.Except(entityToUpdate.BookAuthors).ToList();

            deletedBooks.ForEach(bookToDelete =>
                entityToUpdate.BookAuthors.Remove(
                    entityToUpdate.BookAuthors
                        .First(b => b.BookId == bookToDelete.BookId)));

            foreach (var addedBook in addedBooks)
            {
                _bookStoreContext.Entry(addedBook).State = EntityState.Added;
            }

            _bookStoreContext.SaveChanges();
        }

        public void Delete(Author entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
