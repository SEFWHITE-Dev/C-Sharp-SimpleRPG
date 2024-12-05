using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class ItemQuantity
    {
        // there should be no need to set another value to these parameters outside the constructor, hence no setter
        public int ItemID { get; }
        public int Quantity { get; }

        public ItemQuantity(int itemID, int quantity) 
        { 
            ItemID = itemID;
            Quantity = quantity;
        }
    }
}
