using System.Linq.Expressions;

namespace Dashboard.Extensions;

/// <summary>Утилиты для сортировки</summary>
public class SortUtils
{
    /// <summary>
    ///     Получает свойство сущности <typeparamref name="T" /> по <paramref name="name" />.
    ///     Поддерживает указание вложенных свойств через <c>.</c>
    /// </summary>
    public static Expression<Func<T, object>> GetPropertyAccessor<T>(string name)
    {
        var parameter = Expression.Parameter(typeof(T), "input");
        var propertiesNames = name.Split('.');
        var field = propertiesNames.Aggregate((Expression)parameter, Expression.PropertyOrField);
        var boxed = Expression.ConvertChecked(field, typeof(object));
        return Expression.Lambda<Func<T, object>>(boxed, parameter);
    }
}