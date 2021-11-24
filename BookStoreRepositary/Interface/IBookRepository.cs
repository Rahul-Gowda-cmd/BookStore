using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepositary.Interface
{
    public interface IBookRepository
    {
        BookModel AddBook(BookModel book);
        BookModel UpdateBook(BookModel book);
        bool DeleteBook(int bookId);
        bool AddCustomerFeedBack(FeedbackModel feedbackModel);
        List<FeedbackModel> GetCustomerFeedBack(int bookid);
    }
}
