using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowRuleEngine.Tests.Mock
{
    public class Provider
    {
        #region object used to test the rules
        public static Order Order
        {
            get
            {
                Order order = new Order()
                    {
                        OrderNo = 1,
                        Date = DateTime.Now,
                        Amount = 500,
                        Customer = new Customer()
                        {
                            FirstName = "John",
                            LastName = "Smith"
                        }
                    };
                return order;
            }
        }
        #endregion
    }
}
