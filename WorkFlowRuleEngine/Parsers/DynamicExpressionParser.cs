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
    public class DynamicExpressionParser<T>:IExpressionParser<T>
    {
        private const string regEX = @"\[(.*?)\]";

        private ExpressionField leftField = null;
        
        private Delegate delegateFunction;
        public Delegate Delegate
        {
            get 
            {
                return delegateFunction;
            }
        }
        public void ParseExpression(string expression)
        {
            delegateFunction = ParseInputExpression(expression);
        }
        private List<ExpressionField> fields = null;
        protected List<ExpressionField> ExpressionFields
        {
            get
            {
                if (fields == null)
                {
                    fields = new List<ExpressionField>();
                }
                return fields;
            }
        }
        private Delegate ParseInputExpression(string expression)
        {
            expression = expression.Trim();
            string tempExpression = expression.Replace("==", "..");
            int assertionIndex = tempExpression.IndexOf("=");



            if (string.IsNullOrEmpty(expression))
                throw new Exception("Please provide a rule expression");

            string parsedExpression = CreateExpressionFields(expression, assertionIndex);

            LambdaExpression lambdaExpression = CreateLambdaExpression(parsedExpression);

            Delegate expressionDelegate = lambdaExpression.Compile();

            return expressionDelegate;
        }
        private string CreateExpressionFields(string expression, int assertionIndex)
        {
            if (string.IsNullOrEmpty(expression))
                return null;

            Match m = Regex.Match(expression, regEX, RegexOptions.IgnoreCase);
            if (!m.Success)
                return expression;

            Type objectType = typeof(T);

            int fieldCounter = 0;

            while (m.Success)
            {
                string fieldName = m.Groups[1].Value;
                ExpressionField field = new ExpressionField() { PropertyName = fieldName };

                bool fieldAlreadyAdded = ExpressionFields.Any(item => item.PropertyName == field.PropertyName);

                if (fieldAlreadyAdded == false)
                {
                    ExpressionProperty pInfo = objectType.GetPropertyInfo(field.PropertyName);
                    field.PropertyInfo = pInfo;

                    if (assertionIndex > 0 && fieldCounter == 0)
                    {
                        leftField = field;
                        fieldCounter = 1;
                    }
                    else
                    {
                        fields.Add(field);
                    }

                    expression = expression.Replace(m.Value, field.ParamName);
                }
                m = m.NextMatch();
            }
            if (assertionIndex > 0)
            {
                return expression.Substring(assertionIndex, expression.Length - assertionIndex);
            }
            return expression;
        }
        private LambdaExpression CreateLambdaExpression(string parsedExpression)
        {
            LambdaExpression lambdaExpression = null;
            List<ParameterExpression> parametersList = new List<ParameterExpression>();
            if (fields != null)
            {
                foreach (ExpressionField field in fields)
                {
                    parametersList.Add(field.ParameterExpression);
                }
            }

            lambdaExpression = DynamicQueryable.ParseLambda(parametersList.ToArray(), null, parsedExpression);

            return lambdaExpression;
        }


        public object Evaluate(T input)
        {
            if (fields != null && input == null)
            {
                throw new Exception("Cannot evaluate a null object " );
            }

            object result = null;
            List<object> arguments = new List<object>();
            object propertyValue = null;
            if (fields != null)
            {
                foreach (ExpressionField field in fields)
                {
                    //propertyValue = field.PropertyInfo.PropertyInfo.GetValue(input, null);
                    propertyValue = field.GetPropertyValue(input);
                    arguments.Add(propertyValue);
                }
            }

            result = delegateFunction.DynamicInvoke(arguments.ToArray());

            if (leftField != null)
            {
                //leftField.PropertyInfo.PropertyInfo.SetValue(input, result,null);
                leftField.SetPropertyValue(input, result);
            }
            return result;
        }
    }
}
