using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Interface
{
    public interface ICartManager
    {
        string AddBookToCart(int bookId, int userId);
        List<CartModel> GetCartItems(int userId);
        bool DeleteBookFromCart(int cartId);
        bool UpdateCartItem(int cartId, int quantityToBuy);
    }
}
