using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepositary.Interface
{
    public interface IProductRepository
    {
        List<BookModel> GetAllBooks();
    }
}
