namespace Sharplox.Expr;

public interface IVisitor<T>
{
    T VisitBinaryExpression(Binary expr);
    T VisitGroupingExpression(Grouping expr);
    T VisitLiteralExpression(Literal expr);
    T VisitUnaryExpression(Unary expr);
}