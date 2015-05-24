using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowRuleEngine.Rules
{
    public class ExpressionProperty
    {
        public PropertyInfo PropertyInfo { get; set; }
        public ExpressionProperty NextExpressionProperty { get; set; }

        private ParameterExpression paramExpression = null;
        public ParameterExpression ParameterExpression(string paramName)
        {
            if (paramExpression == null)
            {
                
                ExpressionProperty expProperty = this;
                while (expProperty.NextExpressionProperty != null)
                {
                    expProperty = expProperty.NextExpressionProperty;   
                }

                paramExpression = Expression.Parameter(expProperty.PropertyInfo.PropertyType, paramName);
            }
            return paramExpression;
        }
    }
}
