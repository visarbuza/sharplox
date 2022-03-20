namespace Sharplox.Expr;

public class Grouping : Expr
{
    public Grouping(Expr expression)
    {
        Expression = expression;
    }

    public Expr Expression { get; set; }
    
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitGroupingExpression(this);
    }
}