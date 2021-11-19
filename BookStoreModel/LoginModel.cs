using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStoreModel
{
    public class LoginModel
    {  
        public string email { get; set; }

        public string password { get; set; }
    }
}
