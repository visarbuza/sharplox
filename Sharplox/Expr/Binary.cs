namespace Sharplox.Expr;

public class Binary : Expr
{
    public Expr Left { get; set; }
    public Token Operator { get; set; }
    public Expr Right { get; set; }

    public Binary(Expr left, Token @operator, Expr right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}