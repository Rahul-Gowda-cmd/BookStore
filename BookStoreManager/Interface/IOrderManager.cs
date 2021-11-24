using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Interface
{
    public interface IOrderManager
    {
        bool PlaceTheOrder(List<CartModel> orderdetails);
        List<OrderModel> GetOrderList(int userId);
    }
}
