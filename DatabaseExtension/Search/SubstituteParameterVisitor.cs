using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DatabaseExtension
{
    public class ParameterVisitor : ExpressionVisitor
    {
        private readonly LambdaExpression _expressionToVisit;
        private readonly Dictionary<ParameterExpression, Expression> _substitutionParameterPairs;

        public ParameterVisitor(Expression[] parameterSubstitutions, LambdaExpression expressionToVisit)
        {
            _expressionToVisit = expressionToVisit;
            _substitutionParameterPairs = expressionToVisit.Parameters
                    .Select((parameter, index) => new { Parameter = parameter, Index = index })
                    .ToDictionary(pair => pair.Parameter, pair => parameterSubstitutions[pair.Index]);
        }

        public Expression ExpressionReplace()
        {
            Expression expression = Visit(_expressionToVisit.Body);

            return expression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            bool notPrameterValue = _substitutionParameterPairs.TryGetValue(node, out Expression substitution);

            Expression expression = notPrameterValue
                ? base.Visit(substitution)
                : base.VisitParameter(node);

            return expression;
        }
    }
}
