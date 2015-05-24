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

    public partial  class WorkFlowRuleTestBasic
    {
        

        
        [Test]
        public void SimpleRule()
        {
            Rule<Order> orderRule = new Rule<Order>();
            object rez = null;

            rez = orderRule.Expression("5").Evaluate(null);

            Assert.AreNotEqual(rez, null);
        }

        [Test]
        public void SimpleRuleAddExpression()
        {
            Rule<Order> orderRule = new Rule<Order>();
            object rez = null;
            rez = orderRule.Expression("5+2").Evaluate(null);

            int response = Int32.Parse(rez.ToString());
            Assert.AreEqual(response, 7);
        }

        [Test]
        public void SimpleRuleExpressionPriority()
        {
            Rule<Order> orderRule = new Rule<Order>();
            object rez = null;
            rez = orderRule.Expression("(5+2) * (1*2) ").Evaluate(null);

            int response = Int32.Parse(rez.ToString());
            Assert.AreEqual(response, 14);
        }

    }
}
