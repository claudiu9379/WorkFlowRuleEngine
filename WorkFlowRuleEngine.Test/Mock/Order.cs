using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowRuleEngine.Tests.Mock
{
    public class Order
    {
        public Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public int OrderNo { get; set; }
        public Guid Guid { get; set; }
        
        public decimal Amount { get; set; }
        public int Discount { get; set; }
    }
}
