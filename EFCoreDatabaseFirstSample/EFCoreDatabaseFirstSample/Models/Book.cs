using System;
using System.Collections.Generic;

namespace EFCoreDatabaseFirstSample.Models
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthors>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public long CategoryId { get; set; }
        public long PublisherId { get; set; }

        public virtual BookCategory Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthors> BookAuthors { get; set; }
    }
}
