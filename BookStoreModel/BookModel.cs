using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreModel
{
    public class BookModel
    {
        public int _id { get; set; }

        public string bookName { get; set; }

        public string author { get; set; }

        public string description { get; set; }

        public string bookImage { get; set; }

        public int quantity { get; set; }

        public int price { get; set; }

        public int discountPrice { get; set; }

    }
}
