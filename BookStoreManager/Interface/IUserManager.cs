using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Interface
{
    public interface IUserManager
    {

        UserModel Register(UserModel user);
        string Login(LoginModel user);
        string GenerateToken(string userName);
        string ForgotPassword(string email);
        string ResetPassword(ResetPasswordModel user);
    }
}
