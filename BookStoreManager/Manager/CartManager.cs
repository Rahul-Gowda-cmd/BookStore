using BookStoreManager.Interface;
using BookStoreModel;
using BookStoreRepositary.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Manager
{
    public class CartManager : ICartManager
    {
        private readonly ICartRepository repository;

        // Constructor
        public CartManager(ICartRepository repository)
        {
            this.repository = repository;
        }

        public string AddBookToCart(int bookId, int userId)
        {
            try
            {
                return this.repository.AddBookToCart(bookId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CartModel> GetCartItems(int userId)
        {
            try
            {
                return this.repository.GetCartItems(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdateCartItem(int cartId, int quantityToBuy)
        {
            try
            {
                return this.repository.UpdateCartItem(cartId, quantityToBuy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteBookFromCart(int cartId)
        {
            try
            {
                return this.repository.DeleteBookFromCart(cartId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
