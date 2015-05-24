# WorkFlowRuleEngine
Work flow rule engine created using fluent interface. 

The string expressions are compiled at te moment when the rule receive the string expression. This provides a very fast rule execution.
Rulee can be created using 
 - simple and complex properties (e.g. [Customer.FirstName])
 - string expressions with possibility to combine with C# code (e.g. [FirstName] + [LastName].SubString(0,1))
 - predicates ; see below example
 - setting the input property based on dynamic expression (e.g. [Discount] = [Customer.DiscountPercent] * 0.1) 
 - invoking prebuid actions in case of perdicate validation

The enhancemets are:
- read the rules from an external file (json serialization)
- rule chain
- assertion of property value by using binary expression instead of Reflection
- string compilation of type (it=>it.Discount = it.Customer.DiscountPercent * 0.1); currently this is handled by  [Discount] = [Customer.DiscountPercent] * 0.1

How Do You Use It?
------------------


```csharp
		[Test]
        public void OrderNoAdd()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[OrderNo]+[OrderNo]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, 2);
        }
```

```csharp
		[Test]
        public void CompositePropertyConcatenation()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[Customer.FirstName]+[Customer.FirstName]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, "JohnJohn");
        }
```


```csharp
		[Test]
        public void PredicateTestSimplePropertyExpression()
        {

            object rez = null;
            Rule<Order> orderRule = new Rule<Order>();
            rez = orderRule.Predicate(
              p => p.Condition("[Discount] == 0")
                  .WhenFalse(it=>it.Discount = 1)
                  .WhenTrue("[Discount] = 2")
              ).Evaluate(Provider.Order);

            Order response = (Order)rez;
            Assert.AreEqual(response.Discount, 2);
        }
```


```csharp
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
```