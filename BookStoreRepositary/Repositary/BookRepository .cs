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
    public class BookRepository: IBookRepository
    {
        string connectionString;

        public BookRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
        }

        public BookModel AddBook(BookModel book)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spBookAdd";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@author", book.author);
                    command.Parameters.AddWithValue("@bookImage", book.bookImage);
                    command.Parameters.AddWithValue("@bookName", book.bookName);
                    command.Parameters.AddWithValue("@description", book.description);
                    command.Parameters.AddWithValue("@discountPrice", book.discountPrice);
                    command.Parameters.AddWithValue("@price", book.price);
                    command.Parameters.AddWithValue("@quantity", book.quantity);
                    command.Parameters.Add("@book", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@book"].Value;

                    if (!(id is DBNull))
                    {
                        book._id = Convert.ToInt32(id);
                        return book;
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

        public BookModel UpdateBook(BookModel book)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spBookUpdate";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", book._id);
                    command.Parameters.AddWithValue("@author", book.author == null ? "" : book.author);
                    command.Parameters.AddWithValue("@bookImage", book.bookImage == null ? "" : book.bookImage);
                    command.Parameters.AddWithValue("@bookName", book.bookName == null ? "" : book.bookName);
                    command.Parameters.AddWithValue("@description", book.description == null ? "" : book.description);
                    command.Parameters.AddWithValue("@discountPrice", book.discountPrice);
                    command.Parameters.AddWithValue("@price", book.price);
                    command.Parameters.AddWithValue("@quantity", book.quantity);
                    command.Parameters.Add("@book", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@book"].Value;

                    if (!(id is DBNull))
                    {
                        return book;
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

        public bool DeleteBook(int bookId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spBookDelete";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", bookId);
                    command.Parameters.Add("@book", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@book"].Value;

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
