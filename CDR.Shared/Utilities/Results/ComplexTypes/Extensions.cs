using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace CDR.Shared.Utilities.Results.ComplexTypes
{
    public static class Extentions
    {


        public static IEnumerable<T> SelectNestedChildren<T>
            (this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var item in source)
            {
                yield return item;
                foreach (var subItem in SelectNestedChildren(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }

        public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);
                foreach (var child in SelectRecursive(children, selector))
                    yield return child;
            }
        }

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("Değer Enum olmalıdır", nameof(enumerationValue));
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0)
            {
                return enumerationValue.ToString();
            }

            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumerationValue.ToString();

        }

        public static string PrReplace(this string value, string source, string target)
        {
            return (value ?? "").Replace("\n", "").Replace(source, target);
        }

        public static bool IsNull(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static void CheckVal(this bool? value, string message)
        {
            if (value == true)
            {
                throw new ArgumentException(message);
            }
        }
        public static void CheckVal(this bool value, string message)
        {
            if (value == true)
            {
                throw new ArgumentException(message);
            }
        }


        public static bool IsNull(this int value)
        {
            return value < 1;
        }
        public static bool IsNull(this int? value)
        {
            return (value == null || value < 1);
        }
        public static bool IsNull(this object value)
        {
            return (value == null);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string ToJson(this object value)
        {
            //return Newtonsoft.Json.JsonConvert.SerializeObject(value);
            return "";
        }

        public static decimal ToDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            var decimalSeperator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var allowedChars = "0123456789.,";
            var valueTemp = value;
            value = "";

            foreach (var c in valueTemp)
            {
                if (allowedChars.Contains(c.ToString()))
                {
                    value += c;
                }
            }

            var beforeVal = "";

            value = value.Replace(".", decimalSeperator).Replace(",", decimalSeperator);


            var commasCount = value.Split(decimalSeperator[0]);

            if (commasCount.Length == 1)
            {
                beforeVal = commasCount[0];
            }
            else if (commasCount.Length == 2)
            {
                beforeVal = commasCount[0] + decimalSeperator + commasCount[1];
            }
            else if (commasCount.Length > 2)
            {
                for (var i = 0; i < commasCount.Length - 1; i++)
                {
                    beforeVal += commasCount[i];
                }
                beforeVal += decimalSeperator + commasCount[commasCount.Length - 1];
            }
            else
            {
                beforeVal = "0";
            }


            return Convert.ToDecimal(beforeVal);
        }

        public static int ToInt(this string value, int defaultValue = 0)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long Tolong(this string value, long defaultValue = 0)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this string value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                throw new ArgumentException("Değer DateTime Tipine Dönüştürülemedi");
            }
        }
        public static DateTime ToDateTime(this DateTime? value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static DateTime ToDateTime(this string value, string format)
        {
            try
            {
                if (value.IsNull())
                {
                    return DateTime.MinValue;
                }

                return DateTime.ParseExact(value, format,
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException("Değer Formatlı olan DateTime Tipine Dönüştürülemedi");
            }
        }


        public static string ToNumberString(this int? value, string suffix = "", string prefix = "", int countAfterCommas = 0)
        {
            return (value ?? 0).ToNumberString(suffix: suffix, prefix: prefix, countAfterCommas: countAfterCommas);
        }
        public static string ToNumberString(this int value, string suffix = "", string prefix = "", int countAfterCommas = 0)
        {
            return prefix + " " + (value).ToString("N" + countAfterCommas.ToString()) + " " + suffix;
        }
        public static string ToNumberString(this decimal? value, string suffix = "", string prefix = "", int countAfterCommas = 0)
        {
            return prefix + " " + (value ?? 0).ToString("N" + countAfterCommas.ToString()) + " " + suffix;
        }
        public static string ToNumberString(this decimal value, string suffix = "", string prefix = "", int countAfterCommas = 0)
        {
            return prefix + " " + value.ToString("N" + countAfterCommas.ToString()) + " " + suffix;
        }


        public static string ToDateTimeString(this DateTime value, string pattern = "dd/MM/yyyy HH:mm")
        {
            return value.ToString(pattern);
        }
        public static string ToDateTimeString(this DateTime? value, string pattern = "dd/MM/yyyy HH:mm")
        {
            return (value ?? DateTime.MinValue).ToString(pattern);
        }


        public static string ToOnlyDateString(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy");
        }
        public static string ToOnlyDateString(this DateTime? value)
        {
            return (value ?? DateTime.MinValue).ToString("dd/MM/yyyy");
        }


        public static string ToDatatableDateString(this DateTime? value)
        {
            return (value ?? DateTime.MinValue).ToString("yyyy-MM-dd HH:mm:ss.ffffff");
        }
        public static string ToDatatableDateString(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
        }


        public static IQueryable<TEntity> PrOrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            var command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            if (property == null)
            {
                property = type.GetProperty("CreatedDate");
            }

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static string PrTrim(this string value)
        {
            return (value ?? "").Trim();
        }


    }
}