using System.Collections.Generic;

namespace UI.MainMenu.Book
{
    internal class BookStatusComponent
    {
        public List<BookPage> ActivePages { get; } = new();
    }
}