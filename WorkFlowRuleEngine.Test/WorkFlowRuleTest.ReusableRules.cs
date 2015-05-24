using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowRuleEngine.Rules;
using WorkFlowRuleEngine.Tests.Mock;

namespace WorkFlowRuleEngine.Tests
{

    public partial class ReusableRules
    {
        

        
        [Test]
        public void ReusableRuleTest()
        {
            Rule<Order> orderRule = new Rule<Order>();
            object rez = null;

            orderRule.Expression("[Discount]==0");

            for (int i = 0; i < 1000; i++)
            {
                rez = orderRule.Evaluate(Provider.Order);
                Assert.AreEqual(rez, true);
            }
        }

        

    }
}
