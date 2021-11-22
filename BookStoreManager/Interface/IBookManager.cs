using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Interface
{
    public interface IBookManager
    {
        BookModel AddBook(BookModel book);
        BookModel UpdateBook(BookModel book);
        bool DeleteBook(int bookId);
    }
}
