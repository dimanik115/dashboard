using System.Collections;
using System.Linq.Expressions;
using System.Text.Json;
using Dashboard.Models;

namespace Dashboard.Extensions;

public static class FilterExtensions
{
    public static IEnumerable<T> ExWhere<T>(this IEnumerable<T> source, Filter? filter)
    {
        IEnumerable<string> items = [];
        IEnumerable<object> a = items;
        if (filter == null) return source;
        var predicate = GetPredicate<T>(filter);
        return source.Where(predicate.Compile());
    }

    private static Expression<Func<T, bool>> GetPredicate<T>(Filter filter)
    {
        var parameter = Expression.Parameter(typeof(T), "input");

        Expression tree = filter.LogicalOperation == LogicalOperation.Or
            ? Expression.Constant(false)
            : Expression.Constant(true);

        foreach (var item in filter.Items)
        {
            Expression expression;
            Expression leftProp = Expression.PropertyOrField(parameter, item.Field);
            if (leftProp.Type == typeof(string))
            {
                var toLower = typeof(string).GetMethod("ToLower", [])!;
                var stringReplace = typeof(string).GetMethod(nameof(string.Replace), [typeof(string), typeof(string)])!;
                leftProp = Expression.Call(
                    Expression.Call(leftProp, toLower),
                    stringReplace,
                    Expression.Constant("ё"),
                    Expression.Constant("е")
                );
            }

            Expression? rightValue;
            switch (item.FilterOperation)
            {
                case FilterOperation.Equal:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue == null) continue;
                    expression = Expression.Equal(leftProp, rightValue);
                    break;
                case FilterOperation.NotEqual:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue == null) continue;
                    expression = Expression.NotEqual(leftProp, rightValue);
                    break;
                case FilterOperation.GreaterThan:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue == null) continue;
                    expression = Expression.GreaterThan(leftProp, rightValue);
                    break;
                case FilterOperation.GreaterThanOrEqual:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue == null) continue;
                    expression = Expression.GreaterThanOrEqual(leftProp, rightValue);
                    break;
                case FilterOperation.LessThan:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue == null) continue;
                    expression = Expression.LessThan(leftProp, rightValue);
                    break;
                case FilterOperation.LessThanOrEqual:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue == null) continue;
                    expression = Expression.LessThanOrEqual(leftProp, rightValue);
                    break;
                case FilterOperation.Contains:
                    if (leftProp.Type != typeof(string)) continue;
                    expression = Expression.Call(leftProp,
                        typeof(string).GetMethod("Contains", [typeof(string)])!,
                        Expression.Constant(item.Value.ToString())
                    );
                    break;
                case FilterOperation.In:
                    rightValue = SimpleParse(item.Value, leftProp.Type);
                    if (rightValue is null || !rightValue.Type.IsArray) continue;
                    var method = typeof(Enumerable)
                        .GetMethods()
                        .First(method => method.Name == "Contains" && method.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(object));
                    expression = Expression.Call(method, rightValue, Expression.Convert(leftProp, typeof(object)));
                    break;
                //TODO поиск элемента например в массиве строк (ANY с условием)
                // поиск элемента например в массиве объектов (ANY с условием) + доп вложенность
                default:
                    throw new ArgumentOutOfRangeException();
            }

            tree = filter.LogicalOperation == LogicalOperation.Or
                ? Expression.Or(tree, expression)
                : Expression.And(tree, expression);
        }

        var lambda = Expression.Lambda<Func<T, bool>>(tree, parameter);
        return lambda;
    }

    private static Expression? SimpleParse(object? value, Type targetType)
    {
        if (value == null || (value.GetType().IsClass && value is not string and not IEnumerable)) return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType);
        underlyingType ??= targetType;

        switch (value)
        {
            case JsonElement jsonElement:
            {
                var parsed = ParseJson(jsonElement, underlyingType);
                return parsed switch
                {
                    null => null,
                    IEnumerable => Expression.Constant(parsed),
                    _ => Expression.Constant(parsed, underlyingType) is var expr && underlyingType != targetType
                        ? Expression.Convert(expr, targetType)
                        : expr
                };
            }
            case IEnumerable:
                return Expression.Constant(value);
            default:
                if ((value.GetType() is var valueType &&
                     valueType == underlyingType) || Nullable.GetUnderlyingType(valueType) == underlyingType
                   )
                    return Expression.Constant(value, underlyingType) is var expr && underlyingType != targetType
                        ? Expression.Convert(expr, targetType)
                        : expr;
                return null;
        }
    }

    private static object? ParseJson(JsonElement jsonElement, Type underlyingType)
    {
        if (jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = jsonElement.EnumerateArray().Select(j => ParseJson(j, underlyingType)).ToArray();
            return array.Any(x => x is null) ? null : array;
        }

        if (underlyingType == typeof(string))
            return jsonElement.GetString()!.ToLower().Replace("ё", "е");
        if (underlyingType == typeof(byte))
            return jsonElement.GetByte();
        if (underlyingType == typeof(short))
            return jsonElement.GetInt16();
        if (underlyingType == typeof(int))
            return jsonElement.GetInt32();
        if (underlyingType == typeof(long))
            return jsonElement.GetInt64();
        if (underlyingType == typeof(double))
            return jsonElement.GetDouble();
        if (underlyingType == typeof(float))
            return jsonElement.GetSingle();
        if (underlyingType == typeof(decimal))
            return jsonElement.GetDecimal();
        if (underlyingType == typeof(bool))
            return jsonElement.GetBoolean();
        if (underlyingType == typeof(DateTime))
        {
            var dateTime = jsonElement.GetDateTime();
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
        }

        if (underlyingType == typeof(DateOnly))
            return DateOnly.FromDateTime(jsonElement.GetDateTime());
        if (underlyingType == typeof(Guid))
            return jsonElement.GetGuid();
        if (underlyingType.IsEnum)
            return Enum.Parse(underlyingType, jsonElement.GetString()!);

        return null;
    }

    private static Expression? tmp(object? value, Type targetType)
    {
        if (value == null || (value.GetType().IsClass && value is not string)) return null;

        var underlyingType = Nullable.GetUnderlyingType(targetType);
        underlyingType ??= targetType;

        Expression expression;
        if (value is JsonElement jsonElement)
        {
            if (underlyingType == typeof(string))
                return Expression.Constant(jsonElement.GetString()!.ToLower().Replace("ё", "е"));
            if (underlyingType == typeof(byte))
            {
                expression = Expression.Constant(jsonElement.GetByte());
            }
            else if (underlyingType == typeof(short))
            {
                expression = Expression.Constant(jsonElement.GetInt16());
            }
            else if (underlyingType == typeof(int))
            {
                expression = Expression.Constant(jsonElement.GetInt32());
            }
            else if (underlyingType == typeof(long))
            {
                expression = Expression.Constant(jsonElement.GetInt64());
            }
            else if (underlyingType == typeof(double))
            {
                expression = Expression.Constant(jsonElement.GetDouble());
            }
            else if (underlyingType == typeof(float))
            {
                expression = Expression.Constant(jsonElement.GetSingle());
            }
            else if (underlyingType == typeof(decimal))
            {
                expression = Expression.Constant(jsonElement.GetDecimal());
            }
            else if (underlyingType == typeof(bool))
            {
                expression = Expression.Constant(jsonElement.GetBoolean());
            }
            else if (underlyingType == typeof(DateTime))
            {
                var dateTime = jsonElement.GetDateTime();
                expression = Expression.Constant(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                    dateTime.Minute, dateTime.Second, DateTimeKind.Utc));
            }
            else if (underlyingType == typeof(DateOnly))
            {
                expression = Expression.Constant(DateOnly.FromDateTime(jsonElement.GetDateTime()));
            }
            else if (underlyingType == typeof(Guid))
            {
                expression = Expression.Constant(jsonElement.GetGuid());
            }
            else if (underlyingType.IsEnum)
            {
                expression = Expression.Constant(Enum.Parse(underlyingType, jsonElement.GetString()!), underlyingType);
            }
            else
            {
                return null;
            }
        }
        else
        {
            var valueType = value.GetType();
            if (valueType == underlyingType || Nullable.GetUnderlyingType(valueType) == underlyingType)
                expression = Expression.Constant(value, underlyingType);
            else return null;
        }

        return underlyingType != targetType ? Expression.Convert(expression, targetType) : expression;
    }
}