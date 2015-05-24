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
    
    public partial class WorkFlowRuleTestBasicFields
    {
        

        
        [Test]
        public void ReadOrderNo()
        {
            Rule<Order> orderRule = new Rule<Order>();
            object rez = null;

            rez = orderRule.Expression("[OrderNo]").Evaluate(Provider.Order);

            Assert.AreEqual(rez, 1);
        }

        [Test]
        public void OrderNoAddNullObject()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var ex = Assert.Throws<Exception>(() => orderRule.Expression("[OrderNo]+[OrderNo]").Evaluate(null));
            Assert.AreNotEqual(ex, null);
        }

        [Test]
        public void OrderNoAdd()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[OrderNo]+[OrderNo]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, 2);
        }

        [Test]
        public void PropertyRead()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[OrderNo]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, 1);
        }

        [Test]
        public void CompositePropertyRead()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[Customer.FirstName]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, "John");
        }

        [Test]
        public void CompositePropertyConcatenation()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[Customer.FirstName]+[Customer.FirstName]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, "JohnJohn");
        }

        [Test]
        public void UsingBinarryExpression()
        { }

        [Test]
        public void UsingCompiledFunctionForAssertion()
        {
 
        }
    }
}
