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
    public class OrderRepository: IOrderRepository
    {
        string connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
        }

        public bool PlaceTheOrder(List<CartModel> orderdetails)
        {
            bool res = false;
            SqlConnection connection = new SqlConnection(connectionString);
            using (connection)
                try
                {
                    foreach (var order in orderdetails)
                    {
                        SqlCommand sqlCommand = new SqlCommand("spPlaceOrder", connection);
                        sqlCommand.CommandType =CommandType.StoredProcedure;
                        connection.Open();
                        sqlCommand.Parameters.AddWithValue("@BookId", order.product_id);
                        sqlCommand.Parameters.AddWithValue("@UserId", order.user_id);
                        string date = DateTime.Now.ToString(" dd MMM yyyy");
                        sqlCommand.Parameters.AddWithValue("@OrderDate", date);
                        var returnedSQLParameter = sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                        returnedSQLParameter.Direction = ParameterDirection.Output;
                        sqlCommand.ExecuteNonQuery();
                        int result = (int)returnedSQLParameter.Value;

                        if (result == 1)
                        {
                            res = true;
                            connection.Close();
                        }
                        else
                        {
                            res = false;
                            break;
                        }
                    }
                    return res;
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
        public List<OrderModel> GetOrderList(int userId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            using (connection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("dbo.GetOrderDetails", connection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();
                    sqlCommand.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<OrderModel> orderList = new List<OrderModel>();
                        while (reader.Read())
                        {
                            BookModel booksModel = new BookModel();
                            OrderModel orderModel = new OrderModel();


                            orderModel.BookId = Convert.ToInt32(reader["BookId"]);
                            booksModel.author = reader["AuthorName"].ToString();
                            booksModel.bookName = reader["BookName"].ToString();
                            booksModel.price = Convert.ToInt32(reader["Price"]);
                            booksModel.bookImage = reader["Image"].ToString();
                            booksModel.discountPrice = Convert.ToInt32(reader["OriginalPrice"]);
                            orderModel.OrderId = Convert.ToInt32(reader["OrderId"]);
                            orderModel.DateOfOrder = reader["OrderDate"].ToString();
                            orderModel.Books = booksModel;

                            orderList.Add(orderModel);
                        }
                        return orderList;
                    }
                    else
                    {
                        return null;
                    }
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
