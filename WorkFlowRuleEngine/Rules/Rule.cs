using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Reflection;
using WorkFlowRuleEngine.Helpers;

namespace WorkFlowRuleEngine.Rules
{
    public class Rule<T>
    {
        #region Class Data
        private const string regEX = @"\[(.*?)\]";
        private Delegate expressionDelegate = null;
        private PredicateRuleWrapper<T> predicateRule;
        private ExpressionField leftField = null;
        #endregion

        private string expression = null;
        public string ExpressionValue
        {
            get 
            {
                return expression;
            }
            private set {
                expression = value;
            }
        }

        public Rule<T> Predicate(Action<PredicateRule<T>> predicate)
        {
            this.predicateRule = new PredicateRuleWrapper<T>();
            predicate(this.predicateRule);

            return this;
        }
        #region Properties
        //private LambdaExpression Lambda { get; set; }

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
        protected void Clear()
        {
            if (fields != null)
            {
                fields.Clear();
            }
            expressionDelegate = null;
        }

        public Type GetReturnType()
        {
            if (expressionDelegate == null)
            {
                return null;
            }
            return expressionDelegate.Method.ReturnType;
        }
       
        #endregion

        public virtual Rule<T> Expression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new Exception("Empty expression");


            this.Clear();

            this.ExpressionValue = expression;

            ParseExpression(expression);

            return this;
        }


        #region Parse Expression
        private void ParseExpression(string expression)
        {
            expression = expression.Trim();
            string tempExpression = expression.Replace("==", "..");
            int assertionIndex = tempExpression.IndexOf("=");



            if (string.IsNullOrEmpty(expression))
                throw new Exception("Please provide a rule expression");

            string parsedExpression = CreateExpressionFields(expression, assertionIndex);

            LambdaExpression lambdaExpression = CreateLambdaExpression(parsedExpression);

            expressionDelegate = lambdaExpression.Compile();

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
                    else {
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
        #endregion



        public virtual object Evaluate(T input)
        {

            if (string.IsNullOrEmpty(expression) && predicateRule == null)
            {
                throw new Exception("Please specify a expression or predicate");
            }

            if (predicateRule != null)
            {
               predicateRule.Evaluate(input);
               return input;
            }
            if (string.IsNullOrEmpty(expression))
                return null;

            if (fields != null && input == null)
            {
                throw new Exception("Please pass a object instance to " + ExpressionValue);
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
           
            result = expressionDelegate.DynamicInvoke(arguments.ToArray());

            if (leftField != null)
            {
                //leftField.PropertyInfo.PropertyInfo.SetValue(input, result,null);
                leftField.SetPropertyValue(input, result);
            }
            return result;
        }
    }
}
