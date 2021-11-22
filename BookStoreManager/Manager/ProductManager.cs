using BookStoreManager.Interface;
using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Manager
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository repository;

        // Constructor
        public ProductManager(IProductRepository repository)
        {
            this.repository = repository;
        }

        public List<BookModel> GetAllBooks()
        {
            try
            {
                return this.repository.GetAllBooks();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
