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

    public partial class WorkFlowRuleTestPredicate
    {
        

        [Test]
        public void PredicateTest()
        {
            object rez = null;
            Rule<Order> orderRule = new Rule<Order>();
            rez = orderRule.Predicate(
              p => p.Condition("[Discount] == 0")
                  .WhenFalse(it => it.Customer.FirstName = "False")
                  .WhenTrue(it => it.Customer.FirstName = "True")
              ).Evaluate(Provider.Order);

            Order response = (Order)rez;
            Assert.AreEqual(response.Customer.FirstName, "True");
        }

        [Test]
        public void PredicateTestExpression()
        {
            string delimiter = "\"";

            object rez = null;
            Rule<Order> orderRule = new Rule<Order>();
            rez = orderRule.Predicate(
              p => p.Condition("[Discount] == 0")
                  .WhenFalse("[Customer.FirstName] = " + delimiter + "False" + delimiter)
                  .WhenTrue("[Customer.FirstName] = " + delimiter + "True" + delimiter)
              ).Evaluate(Provider.Order);

            Order response = (Order)rez;
            Assert.AreEqual(response.Customer.FirstName, "True");
        }

        [Test]
        public void PredicateTestSimplePropertyExpression()
        {

            object rez = null;
            Rule<Order> orderRule = new Rule<Order>();
            rez = orderRule.Predicate(
              p => p.Condition("[Discount] == 0")
                  .WhenFalse("[Discount] = 1 " )
                  .WhenTrue("[Discount] = 2")
              ).Evaluate(Provider.Order);

            Order response = (Order)rez;
            Assert.AreEqual(response.Discount, 2);
        }
    }
}
