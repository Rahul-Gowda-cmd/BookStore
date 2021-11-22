using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepositary.Interface
{
    public interface IWishlistRepository
    {
        string AddBookToWishlist(int bookId, int userId);
        List<WishlistModel> GetWishlistItems(int userId);
        bool DeleteBookFromWishlist(int wishlistId);
    }
}
