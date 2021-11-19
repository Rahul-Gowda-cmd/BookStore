using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStoreModel
{
    public class ResetPasswordModel
    {

        public int _id { get; set; }
        
        public string password { get; set; }
     
        public string token { get; set; }
    }
}
