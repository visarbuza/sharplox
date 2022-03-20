namespace Sharplox.Expr;

public abstract class Expr
{
    public abstract T Accept<T>(IVisitor<T> visitor);
}