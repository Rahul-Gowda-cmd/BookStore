using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStoreModel
{
    public class UserModel
    {
 
        public int _id { get; set; }     
        public string fullName { get; set; }
        
        public string email { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
    }
}
