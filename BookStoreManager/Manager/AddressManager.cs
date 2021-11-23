using BookStoreManager.Interface;
using BookStoreModel;
using BookStoreRepositary.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManager.Manager
{
    public class AddressManager : IAddressManager
    {
        private readonly IAddressRepository repository;
        public AddressManager(IAddressRepository repository)
        {
            this.repository = repository;
        }
        public bool AddUserAddress(AddressModel userDetails)
        {
            try
            {
                return this.repository.AddUserAddress(userDetails);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<AddressModel> GetUserDetails(int userId)
        {
            try
            {
                return this.repository.GetUserDetails(userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool EditAddress(AddressModel details)
        {
            try
            {
                return this.repository.EditAddress(details);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
