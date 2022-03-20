using System.Text;

namespace Sharplox.Expr;

public class AstPrinter : IVisitor<string>
{
    public string Print (Expr expr) {
        return expr.Accept(this);
    }
    public string VisitBinaryExpression(Binary expr)
    {
        return Parenthesize(expr.Operator.Lexeme, expr.Left, expr.Right);
    }

    public string VisitGroupingExpression(Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public string VisitLiteralExpression(Literal expr)
    {
        return expr.Value == null ? "nil" : expr.Value.ToString();
    }

    public string VisitUnaryExpression(Unary expr)
    {
        return Parenthesize(expr.Operator.Lexeme, expr.Right);
    }
    
    private string Parenthesize(string name, params Expr[] exprs) {
        var builder = new StringBuilder();

        builder.Append('(').Append(name);
        foreach (var expr in exprs) {
            builder.Append(' ');
            builder.Append(expr.Accept(this));
        }
        builder.Append(')');

        return builder.ToString();
    }
}