using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorkFlowRuleEngine.Facade;
using WorkFlowRuleEngine.Rules;

using WorkFlowRuleEngine.Helpers;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace WorkFlowRuleEngine.Parsers
{
    #warning implement the idea from here: https://github.com/zhucai/lambda-parser
    public class FunctionExpressionParser<T> : IExpressionParser<T>
    {
        public void ParseExpression(string expression)
        {
            throw new NotImplementedException();
        }

        public Delegate Delegate
        {
            get { throw new NotImplementedException(); }
        }

        public object Evaluate(T input)
        {
            throw new NotImplementedException();
        }
    }
}
