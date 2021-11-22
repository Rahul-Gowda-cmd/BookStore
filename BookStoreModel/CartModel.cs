using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreModel
{
    public class CartModel
    {
        public int _id { get; set; }

        public BookModel product_id { get; set; }

        public int user_id { get; set; }

        public int quantityToBuy { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime updatedAt { get; set; }
    }
}
