using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreModel
{
    public class WishlistModel
    {
        public int _id { get; set; }

        public BookModel product_id { get; set; }

        public int user_id { get; set; }
    }
}
