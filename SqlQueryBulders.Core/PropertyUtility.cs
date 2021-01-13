using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlQueryBulders.Core
{
    public static class PropertyUtility
    {
        public static List<string> GetPropertyNames<T>()
        {
            List<string> propertyNameList = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => (!x.PropertyType.IsClass || x.PropertyType == typeof(string) || x.PropertyType == typeof(byte[]))
                    && !Attribute.IsDefined(x, typeof(NotMappedAttribute))
                    )
                .Select(x => x.Name)
                .ToList();

            return propertyNameList;
        }

        public static List<string> GetPropertyNames<T>(Expression<Func<T, object>> properties)
        {
            var propertyNameList = new List<string>();

            if (properties != null)
            {
                var newExp = properties.Body as NewExpression;

                foreach (var argExp in newExp.Arguments)
                {
                    var memberExp = argExp as MemberExpression;
                    propertyNameList.Add(memberExp.Member.Name);
                }
            }

            return propertyNameList;
        }
    }
}
