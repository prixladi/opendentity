using System.Linq.Expressions;

namespace Opendentity.Domain;

public class ReplaceParametersVisitor: ExpressionVisitor
{
    private readonly ParameterExpression replacer;
    private readonly ParameterExpression[] parameters;

    public ReplaceParametersVisitor(ParameterExpression replacer, params ParameterExpression[] parameters)
    {
        this.replacer = replacer;
        this.parameters = parameters;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (parameters.Contains(node))
        {
            return base.VisitParameter(replacer);
        }

        return base.VisitParameter(node);
    }
}
