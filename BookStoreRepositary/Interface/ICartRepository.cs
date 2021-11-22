using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepositary.Interface
{
    public interface ICartRepository
    {
        string AddBookToCart(int bookId, int userId);
        List<CartModel> GetCartItems(int userId);
        bool DeleteBookFromCart(int cartId);
        bool UpdateCartItem(int cartId, int quantityToBuy);
    }
}
