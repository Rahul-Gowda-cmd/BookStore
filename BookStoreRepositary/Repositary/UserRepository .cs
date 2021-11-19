using BookStoreModel;
using BookStoreRepositary.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreRepositary.Repositary
{

    public class UserRepository : IUserRepository
    {
        string connectionString;
        string secretKey;
        

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("UserDbConnection");
            secretKey = configuration["SecretKey"];
        }

        public static string EncryptPass(string password)
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(encode);
        }

        public UserModel Register(UserModel user)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spUserRegisteration";
                    SqlCommand command = new SqlCommand(spName, connection);

                    user.password = EncryptPass(user.password);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@fullName", user.fullName);
                    command.Parameters.AddWithValue("@email", user.email);
                    command.Parameters.AddWithValue("@password", user.password);
                    command.Parameters.AddWithValue("@phone", user.phone);
                    command.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();
                    command.ExecuteNonQuery();

                    var id = command.Parameters["@user"].Value;

                    if (!(id is DBNull))
                    {
                        user._id = Convert.ToInt32(id);
                        return user;
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

        public string Login(LoginModel user)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spUserLogin";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@email", user.email);
                    command.Parameters.AddWithValue("@password", EncryptPass(user.password));
                    command.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();

                    command.ExecuteNonQuery();

                    var id = command.Parameters["@user"].Value;

                    if (!(id is DBNull))
                    {
                        if (Convert.ToInt32(id) == 2)
                        {
                            GetUserDetails(user.email);
                            return "Login Successful";
                        }
                        return "Incorrect Password";
                    }
                    return "Login Failed, User Doesnot Exists";
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

        public void GetUserDetails(string email)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string query = "SELECT * FROM Users WHERE email = @email";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@email", email);
                    connection.Open();

                    UserModel customer = new UserModel();
                    SqlDataReader rd = command.ExecuteReader();

                    if (rd.Read())
                    {
                        // Connection to Redis Server
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();

                        // Set values to the Redis cache
                        database.StringSet(key: "FullName", rd.GetString("fullName"));
                        database.StringSet(key: "Phone", rd.GetString("phone"));
                        database.StringSet(key: "UserId", rd.GetInt32("_id").ToString());
                    }
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

        // Generate Token method
        public string GenerateToken(string userName)
        {
            byte[] key = Convert.FromBase64String(secretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userName)
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public string ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public string ResetPassword(ResetPasswordModel user)
        {
            throw new NotImplementedException();
        }
    }
}
