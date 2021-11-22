using BookStoreModel;
using BookStoreRepositary.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BookStoreRepositary.Repositary
{
    public class WishlistRepository : IWishlistRepository
    {
        string connectionString;

        public WishlistRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
        }

        public string AddBookToWishlist(int bookId, int userId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spWishlistAdd";
                    SqlCommand command = new SqlCommand(spName, connection);

                    DateTime createdAt = DateTime.Now;
                    DateTime updatedAt = DateTime.Now;

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@bookId", bookId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.Add("@wishlist", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@wishlist"].Value;

                    if (!(id is DBNull))
                    {
                        if (Convert.ToInt32(id) == 2)
                        {
                            return "Add Book To Wishlist Successful";
                        }
                        return "Add Book To Wishlist Failed, Book already Exists in Cart";
                    }
                    return "Add Book To Wishlist Failed, No Book with that Id";
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

        public List<WishlistModel> GetWishlistItems(int userId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string query = "spWishlistGet";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId", userId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<WishlistModel> wishlistList = new List<WishlistModel>();
                        while (reader.Read())
                        {
                            WishlistModel wishlist = new WishlistModel();
                            BookModel book = new BookModel();

                            book._id = Convert.ToInt32(reader["product_id"]);
                            book.author = reader["author"].ToString();
                            book.bookName = reader["bookName"].ToString();
                            book.description = reader["description"].ToString();
                            book.bookImage = reader["bookImage"].ToString();
                            book.price = Convert.ToInt32(reader["price"]);
                            book.discountPrice = Convert.ToInt32(reader["discountPrice"]);
                            book.quantity = Convert.ToInt32(reader["quantity"]);
                            wishlist._id = Convert.ToInt32(reader["_id"]);
                            wishlist.user_id = Convert.ToInt32(reader["user_id"]);
                            wishlist.product_id = book;

                            wishlistList.Add(wishlist);
                        }
                        return wishlistList;
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

        public bool DeleteBookFromWishlist(int wishlistId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spWishlistDelete";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", wishlistId);
                    command.Parameters.Add("@wishlist", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@wishlist"].Value;

                    if (!(id is DBNull))
                    {
                        return true;
                    }
                    return false;
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
