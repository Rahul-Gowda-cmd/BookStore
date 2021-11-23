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
    public class AddressRepository: IAddressRepository
    {
        string connectionString;

        public AddressRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
        }

        public bool AddUserAddress(AddressModel userDetails)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "storprocedureAddUserDetails";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@address", userDetails.Address);
                    command.Parameters.AddWithValue("@city", userDetails.City);
                    command.Parameters.AddWithValue("@state", userDetails.State);
                    command.Parameters.AddWithValue("@type", userDetails.Type);
                    command.Parameters.AddWithValue("@userId", userDetails.UserId);
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                        return true;
                    else
                        return false;

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

        public List<AddressModel> GetUserDetails(int userId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spGetUSerDetails";
                    SqlCommand command = new SqlCommand(spName, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    command.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader readData = command.ExecuteReader();
                    List<AddressModel> userdetaillist = new List<AddressModel>();
                    if (readData.HasRows)
                    {
                        while (readData.Read())
                        {
                            AddressModel userDetail = new AddressModel();
                            userDetail.AddressId = readData.GetInt32("AddressId");
                            userDetail.Address = readData.GetString("address");
                            userDetail.City = readData.GetString("city").ToString();
                            userDetail.State = readData.GetString("state");
                            userDetail.Type = readData.GetString("type");
                            userdetaillist.Add(userDetail);
                        }
                    }
                    return userdetaillist;
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
        public bool EditAddress(AddressModel userDetails)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spUpdateUserDetails";
                    SqlCommand sqlCommand = new SqlCommand(spName, connection);
                    connection.Open();

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@address", userDetails.Address);
                    sqlCommand.Parameters.AddWithValue("@city", userDetails.City);
                    sqlCommand.Parameters.AddWithValue("@state", userDetails.State);
                    sqlCommand.Parameters.AddWithValue("@type", userDetails.Type);
                    sqlCommand.Parameters.AddWithValue("@addressID", userDetails.AddressId);
                    sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    sqlCommand.Parameters["@result"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = sqlCommand.Parameters["@result"].Value;
                    if (result.Equals(1))
                        return true;
                    else
                        return false;

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
