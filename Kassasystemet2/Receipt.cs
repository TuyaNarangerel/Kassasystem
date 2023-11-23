using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassasystemet2
{
    public class Receipt
    {
        public List<Product> PurchasedProducts { get; } = new List<Product>();
        public double TotalPrice { get; set; }

        public List<int> PurchasedQuantities { get; set; }
    
        public DateTime CreationTime { get; set; }  
        
        public Receipt() 
        {
            PurchasedQuantities = new List<int>();
            
        }

            
    }

}
