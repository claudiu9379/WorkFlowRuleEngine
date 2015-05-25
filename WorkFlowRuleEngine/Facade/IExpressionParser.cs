using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowRuleEngine.Facade
{
    public interface IExpressionParser<T>
    {
        void ParseExpression(string expression);
        Delegate Delegate { get; }
        object Evaluate(T input);
    }
}
