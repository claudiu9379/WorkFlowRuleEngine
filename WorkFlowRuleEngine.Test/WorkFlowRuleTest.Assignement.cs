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

    public partial class WorkFlowRuleTestAssignement
    {
        

        [Test]
        public void OrderNoAssign()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[OrderNo] = [OrderNo]+[OrderNo]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, 2);
        }

        [Test]
        public void OrderNoAssign1()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[OrderNo] = [OrderNo]+[OrderNo] +2").Evaluate(Provider.Order);
            Assert.AreEqual(rez, 4);
        }

        [Test]
        public void OrderNoAssign2()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[Customer.FirstName] = [Customer.FirstName]+[Customer.FirstName]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, "JohnJohn");
        }

    }
}
