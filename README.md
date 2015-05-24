# WorkFlowRuleEngine
Work flow rule engine

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
        public void OrderNoAssign()
        {
            Rule<Order> orderRule = new Rule<Order>();

            var rez = orderRule.Expression("[OrderNo] = [OrderNo]+[OrderNo]").Evaluate(Provider.Order);
            Assert.AreEqual(rez, 2);
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
                  .WhenFalse("[Discount] = 1 " )
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