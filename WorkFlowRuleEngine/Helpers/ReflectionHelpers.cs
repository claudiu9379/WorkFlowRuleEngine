using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WorkFlowRuleEngine.Rules;

namespace WorkFlowRuleEngine.Helpers
{
    public static class ReflectionHelpers
    {

        public static ExpressionProperty GetPropertyInfo(this Type objectType, string propertyName)
        {
            ExpressionProperty result = null;

            PropertyInfo pInfo = objectType.GetProperty(propertyName);
            if (pInfo != null)
            {
                result = new ExpressionProperty()
                {
                    PropertyInfo = pInfo,
                    NextExpressionProperty = null
                };

                return result;
            }
                

            int dotIndex = propertyName.IndexOf(".");

            if (pInfo == null && dotIndex == -1)
            {
                throw new Exception(string.Concat(propertyName, " does not exists for ", objectType.ToString()));
            }

            List<string> properties = propertyName.Split(".".ToCharArray()).ToList();

            ExpressionProperty lastChain = null;
            for (int i = 0; i < properties.Count; i++)
            {
                if (i == 0)
                {
                    result = objectType.GetPropertyInfo(properties[i]);
                    lastChain = result;
                }
                else {
                    lastChain.NextExpressionProperty = GetPropertyInfo(lastChain.PropertyInfo.PropertyType, properties[i]);

                }
            }

            return result;
        }

    }
}
