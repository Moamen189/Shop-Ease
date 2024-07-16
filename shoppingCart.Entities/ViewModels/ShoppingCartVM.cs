using shoppingCart.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingCart.Entities.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCartDetails> CartsList { get; set; }

        public decimal TotalCarts { get; set; }
    }
}
