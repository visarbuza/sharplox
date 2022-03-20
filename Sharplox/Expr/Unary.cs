namespace Sharplox.Expr;

public class Unary : Expr
{
    public Unary(Token @operator, Expr right)
    {
        Operator = @operator;
        Right = right;
    }

    public Token Operator { get; set; }
    public Expr Right { get; set; }
    
    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}