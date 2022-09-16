using Codecool.CodecoolShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class CartDaoMemory : ICartDao
    {
        private List<Cart> data = new List<Cart>();
        private static CartDaoMemory instance = null;
        public static CartDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new CartDaoMemory();
            }
            return instance;
        }
        public void Add(Cart item)
        {
            foreach (Cart cart in data)
            {
                if (cart.product.Id == item.product.Id)
                {
                    cart.amount++;
                    return;
                }
            }
            data.Add(item);
        }
        public Cart Get(int id)
        {
            return data.Find(x => x.product.Id == id);
        }
        public IEnumerable<Cart> GetAll()
        {
            return data;
        }
        public void Remove(int id)
        {
            data.Remove(Get(id));
        }
        public void RemoveAll()
        {
            data.Clear();
        }

    }
}
