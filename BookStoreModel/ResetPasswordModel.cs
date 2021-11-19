using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookStoreModel
{
    public class ResetPasswordModel
    {
        [Key]
        public int _id { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string token { get; set; }
    }
}
