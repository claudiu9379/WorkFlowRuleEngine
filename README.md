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