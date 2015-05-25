using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Reflection;
using WorkFlowRuleEngine.Helpers;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using WorkFlowRuleEngine.Facade;
using WorkFlowRuleEngine.Parsers;

namespace WorkFlowRuleEngine.Rules
{
    #warning ToDo http://stackoverflow.com/questions/10114841/how-to-create-dynamic-lambda-based-linq-expression-from-a-string-in-c
    public class Rule<T>
    {
        #region Class Data
        private PredicateRuleWrapper<T> predicateRule;
        IExpressionParser<T> expressionParser = null;
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

       
        protected void Clear()
        {
           
            if (expressionParser != null)
            {
                expressionParser = null;
            }
        }

        public Type GetReturnType()
        {
            if (expressionParser == null || expressionParser.Delegate == null)
            {
                return null;
            }
            return expressionParser.Delegate.Method.ReturnType;
        }
       
        #endregion


        public virtual Rule<T> Expression(string expression)
        {
            //System.Linq.Expressions.Expression.Lambda(typeof(int), expression);

            if (string.IsNullOrEmpty(expression))
                throw new Exception("Empty expression");


            this.Clear();

            this.ExpressionValue = expression;

            expressionParser = new DynamicExpressionParser<T>();
            expressionParser.ParseExpression(expression);

            return this;
        }


       



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

            return expressionParser.Evaluate(input);
        }
    }
}
