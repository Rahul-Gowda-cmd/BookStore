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
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace BookStoreRepositary.Repositary
{

    public class UserRepository : IUserRepository
    {
        string connectionString;
        string secretKey;
        //EmailService service;

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
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    string spName = "spUserForgot";
                    SqlCommand command = new SqlCommand(spName, connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    connection.Open();

                    command.ExecuteNonQuery();

                    var id = command.Parameters["@user"].Value;

                    if (!(id is DBNull))
                    {
                        string token = this.GenerateToken(email);

                        // Connection to Redis Server
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();

                        // Set values to the Redis cache
                        database.StringSet(key: Convert.ToInt32(id).ToString(), token);

                        string msgBody = "You are receiving this mail because you(or someone else) have requested the reset of the password for your account.\n\n" +
                                    "Please click on the following link, or paste this into your browser to complete the process:\n\n" +
                                    "http://localhost:4200/reset-password/" + $"{token}/{Convert.ToInt32(id).ToString()}" + "\n\n" +
                                    "If you did not request this, please ignore this email and your password will remain unchanged.\n";


                        this.SendMailSmtp(email, msgBody);
                        return "Email has been sent";
                    }
                    return "User Doesnot Exists";
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

        public void SendMailSmtp(string emailTo, string body)
        {
            try
            {
                // Create new instance of mail message
                MailMessage mailMesage = new MailMessage();

                // Add details for the email
                mailMesage.To.Add(emailTo);
                mailMesage.From = new MailAddress("rahul.prabu.07@gmail.com");
                mailMesage.Subject = "Bookstore";
                mailMesage.Body = body;

                // Configure the mail settings
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                smtpServer.Port = 587;
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new System.Net.NetworkCredential("rahul.prabu.07@gmail.com", "Rahul@1995");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mailMesage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ResetPassword(ResetPasswordModel user)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Connection to Redis Server
                ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                IDatabase database = connectionMultiplexer.GetDatabase();

                string token = database.StringGet(user._id.ToString());

                if (token == user.token)
                {
                    // Update
                    using (connection)
                    {
                        string spName = "spUserReset";
                        SqlCommand command = new SqlCommand(spName, connection);

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", user._id);
                        command.Parameters.AddWithValue("@password", EncryptPass(user.password));
                        connection.Open();

                        command.ExecuteNonQuery();
                    }

                    database.KeyDelete(user._id.ToString());

                    // Save all changes to the database
                    return "Reset Password Successful";
                }

                return "Token Expired";
            }
            catch (ArgumentException ex)
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
