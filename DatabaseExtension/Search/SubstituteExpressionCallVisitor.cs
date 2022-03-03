using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DatabaseExtension
{
    /// <summary>
    /// Визитор вызываемыйй маркером для CallVisitor
    /// </summary>
    public class SubstituteExpressionCallVisitor : ExpressionVisitor
    {
        private readonly MethodInfo _markerDesctiprion;

        public SubstituteExpressionCallVisitor()
        {
            _markerDesctiprion = typeof(VisitorExtension)
                .GetMethod(nameof(VisitorExtension.CallVisitor))
                .GetGenericMethodDefinition();
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            bool isMarkerCall = node.Expression.NodeType == ExpressionType.Call && IsMarker((MethodCallExpression)node.Expression);

            if (isMarkerCall)
            {
                LambdaExpression expressionToVisit = Unwrap((MethodCallExpression)node.Expression);

                ParameterVisitor parameterReplacer = new(
                    node.Arguments.ToArray(),
                    expressionToVisit);

                Expression expressionReplace = parameterReplacer.ExpressionReplace();

                Expression expressionVisit = base.Visit(expressionReplace);

                return expressionVisit;
            }

            return base.VisitInvocation(node);
        }

        private static LambdaExpression Unwrap(MethodCallExpression node)
        {
            System.Delegate compileLambda = Expression
                .Lambda(node.Arguments[0])
                .Compile();

            object lambdaExpression = compileLambda
                .DynamicInvoke();

            return lambdaExpression as LambdaExpression;
        }

        private bool IsMarker(MethodCallExpression node)
        {
            return node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == _markerDesctiprion;
        }
    }
}

