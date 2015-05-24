using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowRuleEngine.Rules
{
    public class PredicateRuleWrapper<T> : PredicateRule<T>
    {
        
        public void Evaluate(T input)
        {
            bool result = EvaluateCondition(input);

            if (result)
            {
                EvaluateInCaseOfTrue(input);
            }else{
                EvaluateInCaseOfFalse(input);
            }
        }


        private bool EvaluateCondition(T input)
        {
            if (predicate == null && ruleCondition == null)
                throw new Exception("Predicate is null");

            bool result = false;

            if (predicate != null)
            {
                result = predicate(input);
            }
            else
            {
                object rez = ruleCondition.Evaluate(input);
                result = bool.Parse(rez.ToString());
            }

            return result;
        }

        private object EvaluateInCaseOfTrue(T input)
        {
            if (inCaseOfTrue == null && ruleInCaseOfTrue == null)
                throw new Exception("Please set the method in case of the method is evaluated to True");

            if (inCaseOfTrue != null)
            {
                inCaseOfTrue(input);
            }
            else
            {
                ruleInCaseOfTrue.Evaluate(input);
            }

            return null;
        }


        private object EvaluateInCaseOfFalse(T input)
        {
            if (inCaseOfFalse == null && ruleInCaseOfFalse == null)
                throw new Exception("Please set the method in case of the method is evaluated to True");

            if (inCaseOfFalse != null)
            {
                inCaseOfFalse(input);
            }
            else
            {
                ruleInCaseOfFalse.Evaluate(input);
            }

            return null;
        }
    }
}
