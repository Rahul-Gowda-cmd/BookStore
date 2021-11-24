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

        public bool AddCustomerFeedBack(FeedbackModel feedbackModel)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            using (connection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("storeprocedureAddFeedback", connection);
                    sqlCommand.CommandType =CommandType.StoredProcedure;
                    connection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", feedbackModel.bookId);
                    sqlCommand.Parameters.AddWithValue("@UserId", feedbackModel.userId);
                    sqlCommand.Parameters.AddWithValue("@Rating", feedbackModel.rating);
                    sqlCommand.Parameters.AddWithValue("@FeedBack", feedbackModel.feedback);


                    int result = sqlCommand.ExecuteNonQuery();

                    if (result > 0)
                        return true;
                    else
                        return false;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }
        }
        public List<FeedbackModel> GetCustomerFeedBack(int bookid)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("StoreProcedurGetCustomerFeedback", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@bookid", bookid);
                List<FeedbackModel> feedbackList = new List<FeedbackModel>();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FeedbackModel feedbackdetails = new FeedbackModel();
                        feedbackdetails.userId = reader.GetInt32(0);
                        feedbackdetails.customerName = reader.GetString("FullName");
                        feedbackdetails.feedback = reader.GetString("Feedback");
                        feedbackdetails.rating = reader.GetDouble("Rating");
                        feedbackList.Add(feedbackdetails);
                    }

                }
                return feedbackList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
