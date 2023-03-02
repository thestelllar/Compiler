using System;

namespace compiler {
    class Program {
        static void Main(string[] args) {
            while(true) {
                Console.Write("> ");
                var line = Console.ReadLine();

                Lexer lexer = new Lexer(line);
                while(true) {
                    var token = lexer.NextToken();
                    if(token.TokenKind == SyntaxKind.EndOfFileToken)
                        break;
                    Console.Write($"{token.TokenKind} '{token.TokenText}' ");
                    if(token.TokenValue != null)
                        Console.Write($"{token.TokenValue}");
                    Console.WriteLine();
                }
            }
        }
    }
    enum SyntaxKind {
        NumberToken,
        PlusToken,
        MinusToken,
        MultiplyToken,
        DivideToken,
        PowerToken,
        OpenParenthesesToken,
        CloseParenthesesToken,
        WhiteSpace,
        ErrorToken,
        EndOfFileToken
    }

    class SyntaxToken {

        /*
        // Properties
        // property is combination of field and method

        // private fields
        private SyntaxKind kind;

        // corresponding public property
        public SyntaxKind Kind {
            get { return kind; }
            set { kind = value; }
        }

        Note: 
            It is a good practice to use the same name for both the property and the private field, 
            but with an uppercase first letter.
        */

        // Automatic / Short-Hand properties : same result as above just less code.
        public SyntaxKind TokenKind { get; }
        public int TokenPosition { get; }
        public string TokenText { get; }
        public object TokenValue { get; }

        // constructor
        public SyntaxToken(SyntaxKind tokenKind, int position, string tokenText, object tokenValue) {
            TokenKind = tokenKind;
            TokenPosition = position;
            TokenText = tokenText;
            TokenValue = tokenValue;
        }

    }

    class Lexer {
        // private variables
        private readonly string _line;
        private int _position;

        // constructor
        public Lexer(string line) {
            _line = line;
        }

        // property
        private char CurrentChar {
            get {
                if(_position >= _line.Length)
                    return '\0';
                return _line[_position];
            }
        }

        
        // methods
        private void Next() {
            _position++;
        }

        public SyntaxToken NextToken() {
            // <numbers>
            // + - * / ^ ( )
            // <whitespaces>

            if(_position >= _line.Length)
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

            if(char.IsDigit(CurrentChar)) {
                string tokenText = "";
                var start = _position;

                while(char.IsDigit(CurrentChar))
                    Next();

                int length = _position - start;
                tokenText = _line.Substring(start, length);
                int.TryParse(tokenText, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, tokenText, value);
            }
            
            if(char.IsWhiteSpace(CurrentChar)) {
                string tokenText = "";
                var start = _position;
            
                while(char.IsWhiteSpace(CurrentChar)) 
                    Next();
                
                int length = _position - start;
                tokenText = _line.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhiteSpace, start, tokenText, null);
            }

            if(CurrentChar == '+') {
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            }
            else if(CurrentChar == '-') {
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            }
            else if(CurrentChar == '*') {
                return new SyntaxToken(SyntaxKind.MultiplyToken, _position++, "*", null);
            }
            else if(CurrentChar == '/') {
                return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
            }
            else if(CurrentChar == '^') {
                return new SyntaxToken(SyntaxKind.PowerToken, _position++, "^", null);
            }
            else if(CurrentChar == '(') {
                return new SyntaxToken(SyntaxKind.OpenParenthesesToken, _position++, "(", null);
            }
            else if(CurrentChar == ')') {
                return new SyntaxToken(SyntaxKind.CloseParenthesesToken, _position++, ")", null);
            }

            return new SyntaxToken(SyntaxKind.ErrorToken, _position++, _line.Substring(_position - 1, 1), null);
        }
    }
}