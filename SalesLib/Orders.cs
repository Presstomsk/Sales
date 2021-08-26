using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib
{
    public class Orders
    {
        public uint Id { get; set; }
        public uint BuyerId { get; set; }
        public uint SellerId { get; set; }
        public string Date { get; set; }
        public uint ProductId { get; set; }
        public uint Amount { get; set; }
        public uint TotalPrice { get; set; }
    }
}
