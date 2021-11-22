using BookStoreModel;
using BookStoreRepositary.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BookStoreRepositary.Repositary
{
    public class ProductRepository : IProductRepository
    {
        string connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
        }

        public List<BookModel> GetAllBooks()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string query = "SELECT * FROM Books";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<BookModel> bookList = new List<BookModel>();
                        while (reader.Read())
                        {
                            BookModel book = new BookModel();
                            book._id = Convert.ToInt32(reader["_id"]);
                            book.author = reader["author"].ToString();
                            book.bookName = reader["bookName"].ToString();
                            book.description = reader["description"].ToString();
                            book.bookImage = reader["bookImage"].ToString();
                            book.price = Convert.ToInt32(reader["price"]);
                            book.discountPrice = Convert.ToInt32(reader["discountPrice"]);
                            book.quantity = Convert.ToInt32(reader["quantity"]);

                            bookList.Add(book);
                        }
                        return bookList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
