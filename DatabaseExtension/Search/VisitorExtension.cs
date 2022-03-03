using System.Linq.Expressions;

namespace DatabaseExtension
{
    public static class VisitorExtension
    {
        /// <summary>
        /// Является маркером для вызова визитора
        /// </summary>
        /// <typeparam name="TFunc">Оборабатываемое выржаение</typeparam>
        /// <param name="_"></param>
        /// <returns></returns>
        public static TFunc CallVisitor<TFunc>(this Expression<TFunc> _)
        {
            return default;
        }

        /// <summary>
        /// Явно вызывает визитора для выражения отмеченного CallVisitor
        /// </summary>
        /// <typeparam name="TFunc"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression<TFunc> VisitorMarker<TFunc>(this Expression<TFunc> expression)
        {
            return (Expression<TFunc>)new SubstituteExpressionCallVisitor().Visit(expression);
        }
    }
}

