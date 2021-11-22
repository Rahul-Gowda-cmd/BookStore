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
    public class CartRepository : ICartRepository
    {
        string connectionString;

        public CartRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
        }

        public string AddBookToCart(int bookId, int userId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spCartAdd";
                    SqlCommand command = new SqlCommand(spName, connection);

                    DateTime createdAt = DateTime.Now;
                    DateTime updatedAt = DateTime.Now;

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@bookId", bookId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.Add("@cart", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@cart"].Value;

                    if (!(id is DBNull))
                    {
                        if (Convert.ToInt32(id) == 2)
                        {
                            return "Add Book To Cart Successful";
                        }
                        return "Add Book To Cart Failed, Book already Exists in Cart";
                    }
                    return "Add Book To Cart Failed, No Book with that Id";
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

        public List<CartModel> GetCartItems(int userId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string query = "spCartGet";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userId", userId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<CartModel> cartList = new List<CartModel>();
                        while (reader.Read())
                        {
                            CartModel cart = new CartModel();
                            BookModel book = new BookModel();

                            book._id = Convert.ToInt32(reader["product_id"]);
                            book.author = reader["author"].ToString();
                            book.bookName = reader["bookName"].ToString();
                            book.description = reader["description"].ToString();
                            book.bookImage = reader["bookImage"].ToString();
                            book.price = Convert.ToInt32(reader["price"]);
                            book.discountPrice = Convert.ToInt32(reader["discountPrice"]);
                            book.quantity = Convert.ToInt32(reader["quantity"]);

                            cart._id = Convert.ToInt32(reader["_id"]);
                            cart.user_id = Convert.ToInt32(reader["user_id"]);
                            cart.product_id = book;
                            cart.quantityToBuy = Convert.ToInt32(reader["quantityToBuy"]);

                            cartList.Add(cart);
                        }
                        return cartList;
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

        public bool UpdateCartItem(int cartId, int quantityToBuy)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spCartUpdate";
                    SqlCommand command = new SqlCommand(spName, connection);

                    DateTime updatedAt = DateTime.Now;

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", cartId);
                    command.Parameters.AddWithValue("@quantity", quantityToBuy);
                    command.Parameters.Add("@cart", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@cart"].Value;

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

        public bool DeleteBookFromCart(int cartId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spCartDelete";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", cartId);
                    command.Parameters.Add("@cart", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@cart"].Value;

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
