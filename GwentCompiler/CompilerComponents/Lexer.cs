namespace GwentCompiler;
public class Lexer : IErrorReporter
{
    private int row;
    private int current;
    private bool isAtEnd { get { return current >= currentSource?.Length; } }
    private List<Token> tokens = new();
    private string? source;
    private string? currentSource;

    public List<Token> Tokenize(string input)
    {
        source = input;
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            row++;
            LineTokenizer(line);
        }
        tokens.Add(new Token("", TokenTypes.EOF, row, current + 1));
        row = 0;
        return tokens;
    }

    private void LineTokenizer(string input)
    {
        currentSource = input;
        current = 0;
        while (!isAtEnd)
        {
            bool Match = false;
            foreach (var token in LexicalComponents.tokenRegexPatterns)
            {
                var regex = token.Value;
                var match = regex.Match(input.Substring(current));

                if (match.Success)
                {
                    if (token.Key != TokenTypes.WhiteSpaces)
                    {
                        tokens.Add(new Token(match.Groups[0].Value, token.Key, row, current + 1));
                    }
                    Match = match.Success;
                    current += match.Length;
                    break;
                }

            }
            if (!Match)
            {
                Advance(input);
                GenerateError("Unespected symbol", new CodeLocation(row, current + 1));
            }
        }
    }

    private void Advance(string input)
    {
        current++;
    }

    public void GenerateError(string message, CodeLocation errorLocation)
    {
        LexicalError newError = new LexicalError(message, errorLocation);

        if (!(Error.AllErrors.Count > 0 && newError.ErrorLocation.Column == IErrorReporter.lastColumn + 1 && newError.ErrorLocation.Row == IErrorReporter.lastRow))
        {
            Error.AllErrors.Add(newError);
            Report(newError);
        }

        IErrorReporter.lastRow = newError.ErrorLocation.Row;
        IErrorReporter.lastColumn = newError.ErrorLocation.Column;
    }

    public void Report(Error error)
    {
        System.Console.WriteLine(error);
    }
}