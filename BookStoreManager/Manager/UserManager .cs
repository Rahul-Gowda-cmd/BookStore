using BookStoreManager.Interface;
using BookStoreModel;
using BookStoreRepositary.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Manager
{
    public class UserManager : IUserManager
    {

        private readonly IUserRepository repository;

        // Constructor
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }

        public UserModel Register(UserModel user)
        {
            try
            {
                return this.repository.Register(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Login(LoginModel user)
        {
            try
            {
                return this.repository.Login(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Gnerate token Method
        public string GenerateToken(string userName)
        {
            try
            {
                return this.repository.GenerateToken(userName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ForgotPassword(string email)
        {
            try
            {
                return this.repository.ForgotPassword(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string ResetPassword(ResetPasswordModel user)
        {
            try
            {
                return this.repository.ResetPassword(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
