using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models
{
    public class Cart
    {
        public Product product { get; set; }
        public int amount { get; set; }
        public int userId { get; set; }
    }
}
