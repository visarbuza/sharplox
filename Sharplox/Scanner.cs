namespace Sharplox;

public class Scanner
{
    private readonly string _source;
    private int _start;
    private int _current;
    private int _line = 1;
    private readonly Dictionary<string, TokenType> _keywords = new()
    {
        {"and", TokenType.And},
        {"class", TokenType.Class},
        {"else", TokenType.Else},
        {"false", TokenType.False},
        {"for", TokenType.For},
        {"fun", TokenType.Fun},
        {"if", TokenType.If},
        {"nil", TokenType.Nil},
        {"or", TokenType.Or},
        {"print", TokenType.Print},
        {"return", TokenType.Return},
        {"super", TokenType.Super},
        {"this", TokenType.This},
        {"true", TokenType.True},
        {"var", TokenType.Var},
        {"while", TokenType.While}
    };

    public Scanner(string source)
    {
        _source = source;
    }

    private List<Token> Tokens { get; set; } = new();

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }
        
        Tokens.Add(new Token(TokenType.Eof, "", null,_line));
        return Tokens;
    }
    
    private char Advance() {
        return _source[_current++];
    }

    private void AddToken(TokenType type) {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object? literal) {
        var text = _source.Substring(_start, _current);
        Tokens.Add(new Token(type, text, literal, _line));
    }
    
    private void ScanToken() {
        var c = Advance();
        switch (c) {
            case '(': AddToken(TokenType.LeftParen); break;
            case ')': AddToken(TokenType.RightParen); break;
            case '{': AddToken(TokenType.LeftBrace); break;
            case '}': AddToken(TokenType.RightBrace); break;
            case ',': AddToken(TokenType.Comma); break;
            case '.': AddToken(TokenType.Dot); break;
            case '-': AddToken(TokenType.Minus); break;
            case '+': AddToken(TokenType.Plus); break;
            case ';': AddToken(TokenType.Semicolon); break;
            case '*': AddToken(TokenType.Star); break; 
            case '!':
                AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                break;
            case '/':
                if (Match('/')) {
                    // A comment goes until the end of the line.
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                } else {
                    AddToken(TokenType.Slash);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;
            case '\n':
                _line++;
                break;
            case '"': Loxstring(); break;
            default:
                if (char.IsDigit(c))
                    Number();
                else if (IsAlpha(c))
                    Identifier();
                else 
                    SharpLox.Error(_line, "Unexpected character.");
                break;
        }
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();
        
        var text = _source.Substring(_start, _current);
        var type = TokenType.Identifier;
        if (_keywords.ContainsKey(text))
            type = _keywords[text];
        AddToken(type);
    }

    private bool IsAlpha(char c)
    {
        return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    }
    
    private bool IsAlphaNumeric(char c) {
        return IsAlpha(c) || char.IsDigit(c);
    }

    private void Number()
    {
        while (char.IsDigit(Peek())) Advance();
        
        // Look for a fractional part.
        if (Peek() == '.' && char.IsDigit(PeekNext())) {
            // Consume the "."
            Advance(); 
            while (char.IsDigit(Peek())) Advance();
        }
        
        AddToken(TokenType.Number, double.Parse(_source.Substring(_start, _current)));
    }

    private void Loxstring() {
        while (Peek() != '"' && !IsAtEnd()) {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd()) {
            SharpLox.Error(_line, "Unterminated string.");
            return;
        }
        
        Advance();
        
        // Trim the surrounding quotes.
        var value = _source.Substring(_start + 1, _current - 1);
        AddToken(TokenType.String, value);
    }

    private char Peek()
    {
        return IsAtEnd() ? '\0' : _source[_current];
    }
    
    private char PeekNext()
    {
        return _current + 1 >= _source.Length ? '\0' : _source[_current + 1];
    } 
    
    private bool Match(char expected) {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;

        _current++;
        return true;
    }
    
    private bool IsAtEnd() {
        return _current >= _source.Length;
    }
}