namespace DBOpen.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ModelProperty<T>
    {
        private static Dictionary<string, Func<T, object>> geterList;
        private static Dictionary<string, Action<T, object>> seterList;

        static ModelProperty()
        {
            ModelProperty<T>.seterList = new Dictionary<string, Action<T, object>>();
            ModelProperty<T>.geterList = new Dictionary<string, Func<T, object>>();
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                ParameterExpression expression;
                ParameterExpression expression2;
                UnaryExpression expression3 = Expression.Convert(expression2 = Expression.Parameter(typeof(object), "val"), info.PropertyType);
                Action<T, object> action = Expression.Lambda<Action<T, object>>(Expression.Call(expression = Expression.Parameter(typeof(T), "obj"), info.GetSetMethod(), new Expression[] { expression3 }), new ParameterExpression[] { expression, expression2 }).Compile();
                Func<T, object> func = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(expression, info), typeof(object)), new ParameterExpression[] { expression }).Compile();
                ModelProperty<T>.seterList.Add(info.Name, action);
                ModelProperty<T>.geterList.Add(info.Name, func);
            }
        }

        public static object GetValue(T model, string propertyName)
        {
            return ModelProperty<T>.geterList[propertyName](model);
        }

        public static void SetValue(T model, string propertyName, object propertyValue)
        {
            try
            {
                ModelProperty<T>.seterList[propertyName](model, propertyValue);
            }
            catch (Exception exception)
            {
                throw new Exception(propertyName + ">>" + exception.Message);
            }
        }
    }
}

