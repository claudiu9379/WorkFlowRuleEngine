using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace WorkFlowRuleEngine.Rules
{
    public class ExpressionField
    {
        public string PropertyName = null;

        public string ParamName
        {
            get
            {
                return PropertyName.Replace(".", "_");
            }
        }

        private ExpressionProperty propInfo = null;
        public ExpressionProperty PropertyInfo
        {
            get { return propInfo; }
            set 
            {
                propInfo = value;
            }
        }
        public object GetPropertyValue(object input)
        {
            object result = PropertyInfo.PropertyInfo.GetValue(input,null);
            ExpressionProperty expProperty = PropertyInfo.NextExpressionProperty;
            while (expProperty != null)
            {
                result = expProperty.PropertyInfo.GetValue(result, null);
                
                expProperty = expProperty.NextExpressionProperty;
            }

            return result;
        }

        public void SetPropertyValue(object input, object value)
        {
            ExpressionProperty expProperty = PropertyInfo;
            while (expProperty.NextExpressionProperty != null)
            {
                input = expProperty.PropertyInfo.GetValue(input, null);

                expProperty = expProperty.NextExpressionProperty;
                
            }
            expProperty.PropertyInfo.SetValue(input, value);
        }

        public ParameterExpression ParameterExpression
        {
            get 
            {
                if (PropertyInfo == null)
                    return null;
                return PropertyInfo.ParameterExpression(ParamName);
            }
        }
    }
}
