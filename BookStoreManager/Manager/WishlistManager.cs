using BookStoreManager.Interface;
using BookStoreModel;
using BookStoreRepositary.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Manager
{
    public class WishlistManager : IWishlistManager
    {
        private readonly IWishlistRepository repository;

        // Constructor
        public WishlistManager(IWishlistRepository repository)
        {
            this.repository = repository;
        }

        public string AddBookToWishlist(int bookId, int userId)
        {
            try
            {
                return this.repository.AddBookToWishlist(bookId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<WishlistModel> GetWishlistItems(int userId)
        {
            try
            {
                return this.repository.GetWishlistItems(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteBookFromWishlist(int wishlistId)
        {
            try
            {
                return this.repository.DeleteBookFromWishlist(wishlistId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
