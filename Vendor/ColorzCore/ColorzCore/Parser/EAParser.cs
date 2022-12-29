using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ColorzCore.DataTypes;
using ColorzCore.IO;
using ColorzCore.Lexer;
using ColorzCore.Parser.AST;
using ColorzCore.Parser.Macros;
using ColorzCore.Preprocessor;
using ColorzCore.Raws;

//TODO: Make errors less redundant (due to recursive nature, many paths will give several redundant errors).

namespace ColorzCore.Parser
{
    internal class EaParser
    {
        public static readonly HashSet<string> SpecialCodes = new HashSet<string>
            { "ORG", "PUSH", "POP", "MESSAGE", "WARNING", "ERROR", "ASSERT", "PROTECT", "ALIGN" };

        private static readonly Dictionary<TokenType, int> Precedences = new Dictionary<TokenType, int>
        {
            { TokenType.MUL_OP, 3 },
            { TokenType.DIV_OP, 3 },
            { TokenType.ADD_OP, 4 },
            { TokenType.SUB_OP, 4 },
            { TokenType.LSHIFT_OP, 5 },
            { TokenType.RSHIFT_OP, 5 },
            { TokenType.SIGNED_RSHIFT_OP, 5 },
            { TokenType.AND_OP, 8 },
            { TokenType.XOR_OP, 9 },
            { TokenType.OR_OP, 10 },
            { TokenType.MOD_OP, 3 }
        };

        private readonly DirectiveHandler _directiveHandler;
        private int _currentOffset;
        private Token _head; //TODO: Make this make sense

        public Log log;
        private bool _offsetInitialized; // false until first ORG, used to warn about writing before first org 

        private readonly Stack<Tuple<int, bool>> _pastOffsets; // currentOffset, offsetInitialized
        private readonly IList<Tuple<int, int, Location>> _protectedRegions;
        private bool _validOffset;

        public EaParser(Dictionary<string, IList<Raw>> raws, Log log, DirectiveHandler directiveHandler)
        {
            GlobalScope = new ImmutableStack<Closure>(new BaseClosure(this), ImmutableStack<Closure>.Nil);
            _pastOffsets = new Stack<Tuple<int, bool>>();
            _protectedRegions = new List<Tuple<int, int, Location>>();
            this.log = log;
            Raws = raws;
            CurrentOffset = 0;
            _validOffset = true;
            _offsetInitialized = false;
            Macros = new MacroCollection(this);
            Definitions = new Dictionary<string, Definition>();
            Inclusion = ImmutableStack<bool>.Nil;
            this._directiveHandler = directiveHandler;

            Pool = new Pool();
        }

        public MacroCollection Macros { get; }
        public Dictionary<string, Definition> Definitions { get; }

        public Dictionary<string, IList<Raw>> Raws { get; }

        //public static readonly HashSet<string> BuiltInMacros = new HashSet<string> { "String", "AddToPool" };
        //TODO: Built in macros.
        //public static readonly Dictionary<string, BuiltInMacro(?)> BuiltInMacros;
        public ImmutableStack<Closure> GlobalScope { get; }

        public int CurrentOffset
        {
            get => _currentOffset;
            private set
            {
                if (value > 0x2000000)
                {
                    if (_validOffset) //Error only the first time.
                    {
                        Error(_head == null ? new Location?() : _head.Location, "Invalid offset: " + value.ToString("X"));
                        _validOffset = false;
                    }
                }
                else
                {
                    _currentOffset = value;
                    _validOffset = true;
                    _offsetInitialized = true;
                }
            }
        }

        public ImmutableStack<bool> Inclusion { get; set; }

        public Pool Pool { get; }

        public bool IsIncluding
        {
            get
            {
                var acc = true;
                for (var temp = Inclusion; !temp.IsEmpty && acc; temp = temp.Tail)
                    acc &= temp.Head;
                return acc;
            }
        }

        public bool IsReservedName(string name)
        {
            return Raws.ContainsKey(name.ToUpper()) || SpecialCodes.Contains(name.ToUpper());
        }

        public bool IsValidDefinitionName(string name)
        {
            return !(Definitions.ContainsKey(name) || IsReservedName(name));
        }

        public bool IsValidMacroName(string name, int paramNum)
        {
            return !Macros.HasMacro(name, paramNum) && !IsReservedName(name);
        }

        public bool IsValidLabelName(string name)
        {
            return true; //!IsReservedName(name);
            //TODO?
        }

        public IList<ILineNode> ParseAll(IEnumerable<Token> tokenStream)
        {
            //TODO: Make BlockNode or EAProgramNode?
            //Note must be strict to get all information on the closure before evaluating terms.
            IList<ILineNode> myLines = new List<ILineNode>();
            var tokens = new MergeableGenerator<Token>(tokenStream);
            tokens.MoveNext();
            while (!tokens.Eos)
                if (tokens.Current.Type != TokenType.NEWLINE || tokens.MoveNext())
                {
                    var retVal = ParseLine(tokens, GlobalScope);
                    retVal.IfJust(n => myLines.Add(n));
                }

            return myLines;
        }

        private BlockNode ParseBlock(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes)
        {
            var start = tokens.Current.Location;
            tokens.MoveNext();
            var temp = new BlockNode();
            IMaybe<ILineNode> x;
            while (!tokens.Eos && tokens.Current.Type != TokenType.CLOSE_BRACE)
                if (!(x = ParseLine(tokens, scopes)).IsNothing)
                    temp.Children.Add(x.FromJust);
            if (!tokens.Eos)
                tokens.MoveNext();
            else
                Error(start, "Unmatched brace.");
            return temp;
        }

        private IMaybe<StatementNode> ParseStatement(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes)
        {
            while (ExpandIdentifier(tokens, scopes))
            {
            }

            _head = tokens.Current;
            tokens.MoveNext();
            //TODO: Replace with real raw information, and error if not valid.
            IList<IParamNode> parameters;
            //TODO: Make intelligent to reject malformed parameters.
            //TODO: Parse parameters after checking code validity.
            if (tokens.Current.Type != TokenType.NEWLINE && tokens.Current.Type != TokenType.SEMICOLON)
            {
                parameters = ParseParamList(tokens, scopes);
            }
            else
            {
                parameters = new List<IParamNode>();
                tokens.MoveNext();
            }

            var upperCodeIdentifier = _head.Content.ToUpperInvariant();

            if (SpecialCodes.Contains(upperCodeIdentifier))
            {
                switch (upperCodeIdentifier)
                {
                    case "ORG":
                        if (parameters.Count != 1)
                            Error(_head.Location, "Incorrect number of parameters in ORG: " + parameters.Count);
                        else
                            parameters[0].TryEvaluate().Case(
                                temp =>
                                {
                                    if (temp > 0x2000000)
                                        Error(parameters[0].MyLocation,
                                            "Tried to set offset to 0x" + temp.ToString("X"));
                                    else
                                        CurrentOffset = temp;
                                },
                                err => { Error(parameters[0].MyLocation, err); });
                        break;
                    case "PUSH":
                        if (parameters.Count != 0)
                            Error(_head.Location, "Incorrect number of parameters in PUSH: " + parameters.Count);
                        else
                            _pastOffsets.Push(new Tuple<int, bool>(CurrentOffset, _offsetInitialized));
                        break;
                    case "POP":
                        if (parameters.Count != 0)
                        {
                            Error(_head.Location, "Incorrect number of parameters in POP: " + parameters.Count);
                        }
                        else if (_pastOffsets.Count == 0)
                        {
                            Error(_head.Location, "POP without matching PUSH.");
                        }
                        else
                        {
                            var tuple = _pastOffsets.Pop();

                            CurrentOffset = tuple.Item1;
                            _offsetInitialized = tuple.Item2;
                        }

                        break;
                    case "MESSAGE":
                        Message(_head.Location, PrettyPrintParams(parameters));
                        break;
                    case "WARNING":
                        Warning(_head.Location, PrettyPrintParams(parameters));
                        break;
                    case "ERROR":
                        Error(_head.Location, PrettyPrintParams(parameters));
                        break;
                    case "ASSERT":
                        if (parameters.Count != 1)
                            Error(_head.Location, "Incorrect number of parameters in ASSERT: " + parameters.Count);
                        else
                            parameters[0].TryEvaluate().Case(
                                temp =>
                                {
                                    if (temp < 0)
                                        Error(parameters[0].MyLocation, "Assertion error: " + temp);
                                },
                                err => { Error(parameters[0].MyLocation, err); });
                        break;
                    case "PROTECT":
                        if (parameters.Count == 1)
                        {
                            parameters[0].TryEvaluate().Case(
                                temp =>
                                {
                                    _protectedRegions.Add(new Tuple<int, int, Location>(temp, 4, _head.Location));
                                },
                                err => { Error(parameters[0].MyLocation, err); });
                        }
                        else if (parameters.Count == 2)
                        {
                            int start = 0, end = 0;
                            var errorOccurred = false;
                            parameters[0].TryEvaluate().Case(
                                temp => { start = temp; },
                                err =>
                                {
                                    Error(parameters[0].MyLocation, err);
                                    errorOccurred = true;
                                });
                            parameters[1].TryEvaluate().Case(
                                temp => { end = temp; },
                                err =>
                                {
                                    Error(parameters[1].MyLocation, err);
                                    errorOccurred = true;
                                });
                            if (!errorOccurred)
                            {
                                var length = end - start;
                                if (length > 0)
                                    _protectedRegions.Add(new Tuple<int, int, Location>(start, length, _head.Location));
                                else
                                    Warning(_head.Location,
                                        "Protected region not valid (end offset not after start offset). No region protected.");
                            }
                        }
                        else
                        {
                            Error(_head.Location, "Incorrect number of parameters in PROTECT: " + parameters.Count);
                        }

                        break;
                    case "ALIGN":
                        if (parameters.Count != 1)
                            Error(_head.Location, "Incorrect number of parameters in ALIGN: " + parameters.Count);
                        else
                            parameters[0].TryEvaluate().Case(
                                temp =>
                                {
                                    CurrentOffset = CurrentOffset % temp != 0
                                        ? CurrentOffset + temp - CurrentOffset % temp
                                        : CurrentOffset;
                                },
                                err => { Error(parameters[0].MyLocation, err); });
                        break;
                }

                return new Nothing<StatementNode>();
            }

            if (Raws.ContainsKey(upperCodeIdentifier))
            {
                //TODO: Check for matches. Currently should type error.
                foreach (var r in Raws[upperCodeIdentifier])
                    if (r.Fits(parameters))
                    {
                        if (CurrentOffset % r.OffsetMod != 0)
                            Error(_head.Location, string.Format("Bad code alignment (offset: {0:X8})", CurrentOffset));

                        StatementNode temp = new RawNode(r, _head, CurrentOffset, parameters);
                        temp.Simplify();

                        CheckDataWrite(temp.Size);
                        CurrentOffset +=
                            temp.Size; //TODO: more efficient spacewise to just have contiguous writing and not an offset with every line?

                        return new Just<StatementNode>(temp);
                    }

                //TODO: Better error message (a la EA's ATOM ATOM [ATOM,ATOM])
                Error(_head.Location, "Incorrect parameters in raw " + _head.Content + '.');
                IgnoreRestOfStatement(tokens);
                return new Nothing<StatementNode>();
            }

            //TODO: Move outside of this else.
            Error(_head.Location, "Unrecognized code: " + _head.Content);
            return new Nothing<StatementNode>();
        }

        public IList<IList<Token>> ParseMacroParamList(MergeableGenerator<Token> tokens)
        {
            IList<IList<Token>> parameters = new List<IList<Token>>();
            var parenNestings = 0;
            do
            {
                tokens.MoveNext();
                var currentParam = new List<Token>();
                while (
                    !(parenNestings == 0 && (tokens.Current.Type == TokenType.CLOSE_PAREN ||
                                             tokens.Current.Type == TokenType.COMMA))
                    && tokens.Current.Type != TokenType.NEWLINE)
                {
                    if (tokens.Current.Type == TokenType.CLOSE_PAREN)
                        parenNestings--;
                    else if (tokens.Current.Type == TokenType.OPEN_PAREN)
                        parenNestings++;
                    currentParam.Add(tokens.Current);
                    tokens.MoveNext();
                }

                parameters.Add(currentParam);
            } while (tokens.Current.Type != TokenType.CLOSE_PAREN && tokens.Current.Type != TokenType.NEWLINE);

            if (tokens.Current.Type != TokenType.CLOSE_PAREN || parenNestings != 0)
                Error(tokens.Current.Location, "Unmatched open parenthesis.");
            else
                tokens.MoveNext();
            return parameters;
        }

        private IList<IParamNode> ParseParamList(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes,
            bool expandFirstDef = true)
        {
            IList<IParamNode> paramList = new List<IParamNode>();
            var first = true;
            while (tokens.Current.Type != TokenType.NEWLINE && tokens.Current.Type != TokenType.SEMICOLON &&
                   !tokens.Eos)
            {
                var head = tokens.Current;
                ParseParam(tokens, scopes, expandFirstDef || !first).IfJust(
                    n => paramList.Add(n),
                    () => Error(head.Location, "Expected parameter."));
                first = false;
            }

            if (tokens.Current.Type == TokenType.SEMICOLON)
                tokens.MoveNext();
            return paramList;
        }

        private IList<IParamNode> ParsePreprocParamList(MergeableGenerator<Token> tokens,
            ImmutableStack<Closure> scopes)
        {
            var temp = ParseParamList(tokens, scopes, false);
            for (var i = 0; i < temp.Count; i++)
                if (temp[i].Type == ParamType.STRING && ((StringNode)temp[i]).IsValidIdentifier())
                    temp[i] = ((StringNode)temp[i]).ToIdentifier(scopes);
            return temp;
        }

        private IMaybe<IParamNode> ParseParam(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes,
            bool expandDefs = true)
        {
            var head = tokens.Current;
            switch (tokens.Current.Type)
            {
                case TokenType.OPEN_BRACKET:
                    return new Just<IParamNode>(new ListNode(head.Location, ParseList(tokens, scopes)));
                case TokenType.STRING:
                    tokens.MoveNext();
                    return new Just<IParamNode>(new StringNode(head));
                case TokenType.MAYBE_MACRO:
                    //TODO: Move this and the one in ExpandId to a separate ParseMacroNode that may return an Invocation.
                    if (expandDefs && ExpandIdentifier(tokens, scopes)) return ParseParam(tokens, scopes);

                    tokens.MoveNext();
                    var param = ParseMacroParamList(tokens);
                    //TODO: Smart errors if trying to redefine a macro with the same num of params.
                    return new Just<IParamNode>(new MacroInvocationNode(this, head, param, scopes));
                case TokenType.IDENTIFIER:
                    if (expandDefs && Definitions.ContainsKey(head.Content) && ExpandIdentifier(tokens, scopes))
                        return ParseParam(tokens, scopes, expandDefs);
                    return ParseAtom(tokens, scopes, expandDefs).Fmap(x => (IParamNode)x);
                default:
                    return ParseAtom(tokens, scopes, expandDefs).Fmap(x => (IParamNode)x);
            }
        }


        private IMaybe<IAtomNode> ParseAtom(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes,
            bool expandDefs = true)
        {
            //Use Shift Reduce Parsing
            var head = tokens.Current;
            var grammarSymbols = new Stack<IEither<IAtomNode, Token>>();
            var ended = false;
            while (!ended)
            {
                bool shift = false, lookingForAtom = grammarSymbols.Count == 0 || grammarSymbols.Peek().IsRight;
                var lookAhead = tokens.Current;

                if (!ended &&
                    !lookingForAtom) //Is already a complete node. Needs an operator of matching precedence and a node of matching prec to reduce.
                    //Verify next symbol to be an operator.
                    switch (lookAhead.Type)
                    {
                        case TokenType.MUL_OP:
                        case TokenType.DIV_OP:
                        case TokenType.MOD_OP:
                        case TokenType.ADD_OP:
                        case TokenType.SUB_OP:
                        case TokenType.LSHIFT_OP:
                        case TokenType.RSHIFT_OP:
                        case TokenType.SIGNED_RSHIFT_OP:
                        case TokenType.AND_OP:
                        case TokenType.XOR_OP:
                        case TokenType.OR_OP:
                            if (Precedences.ContainsKey(lookAhead.Type))
                                Reduce(grammarSymbols, Precedences[lookAhead.Type]);
                            shift = true;
                            break;
                        default:
                            ended = true;
                            break;
                    }
                else if (!ended) //Is just an operator. Error if two operators in a row.
                    //Error if two operators in a row.
                    switch (lookAhead.Type)
                    {
                        case TokenType.IDENTIFIER:
                        case TokenType.MAYBE_MACRO:
                        case TokenType.NUMBER:
                            shift = true;
                            break;
                        case TokenType.OPEN_PAREN:
                        {
                            tokens.MoveNext();
                            var interior = ParseAtom(tokens, scopes);
                            if (tokens.Current.Type != TokenType.CLOSE_PAREN)
                            {
                                Error(tokens.Current.Location,
                                    "Unmatched open parenthesis (currently at " + tokens.Current.Type + ").");
                                return new Nothing<IAtomNode>();
                            }

                            if (interior.IsNothing)
                            {
                                Error(lookAhead.Location, "Expected expression inside paretheses. ");
                                return new Nothing<IAtomNode>();
                            }

                            grammarSymbols.Push(
                                new Left<IAtomNode, Token>(new ParenthesizedAtomNode(lookAhead.Location,
                                    interior.FromJust)));
                            tokens.MoveNext();
                            break;
                        }
                        case TokenType.SUB_OP:
                        {
                            //Assume unary negation.
                            tokens.MoveNext();
                            var interior = ParseAtom(tokens, scopes);
                            if (interior.IsNothing)
                            {
                                Error(lookAhead.Location, "Expected expression after negation. ");
                                return new Nothing<IAtomNode>();
                            }

                            grammarSymbols.Push(
                                new Left<IAtomNode, Token>(new NegationNode(lookAhead, interior.FromJust)));
                            break;
                        }
                        case TokenType.COMMA:
                            Error(lookAhead.Location, "Unexpected comma (perhaps unrecognized macro invocation?).");
                            IgnoreRestOfStatement(tokens);
                            return new Nothing<IAtomNode>();
                        case TokenType.MUL_OP:
                        case TokenType.DIV_OP:
                        case TokenType.MOD_OP:
                        case TokenType.ADD_OP:
                        case TokenType.LSHIFT_OP:
                        case TokenType.RSHIFT_OP:
                        case TokenType.SIGNED_RSHIFT_OP:
                        case TokenType.AND_OP:
                        case TokenType.XOR_OP:
                        case TokenType.OR_OP:
                        default:
                            Error(lookAhead.Location,
                                "Expected identifier or literal, got " + lookAhead.Type + ": " + lookAhead.Content +
                                '.');
                            IgnoreRestOfStatement(tokens);
                            return new Nothing<IAtomNode>();
                    }

                if (shift)
                {
                    if (lookAhead.Type == TokenType.IDENTIFIER)
                    {
                        if (expandDefs && ExpandIdentifier(tokens, scopes))
                            continue;
                        if (lookAhead.Content.ToUpper() == "CURRENTOFFSET")
                            grammarSymbols.Push(new Left<IAtomNode, Token>(new NumberNode(lookAhead, CurrentOffset)));
                        else
                            grammarSymbols.Push(new Left<IAtomNode, Token>(new IdentifierNode(lookAhead, scopes)));
                    }
                    else if (lookAhead.Type == TokenType.MAYBE_MACRO)
                    {
                        ExpandIdentifier(tokens, scopes);
                        continue;
                    }
                    else if (lookAhead.Type == TokenType.NUMBER)
                    {
                        grammarSymbols.Push(new Left<IAtomNode, Token>(new NumberNode(lookAhead)));
                    }
                    else if (lookAhead.Type == TokenType.ERROR)
                    {
                        Error(lookAhead.Location, string.Format("Unexpected token: {0}", lookAhead.Content));
                        tokens.MoveNext();
                        return new Nothing<IAtomNode>();
                    }
                    else
                    {
                        grammarSymbols.Push(new Right<IAtomNode, Token>(lookAhead));
                    }

                    tokens.MoveNext();
                }
            }

            while (grammarSymbols.Count > 1) Reduce(grammarSymbols, 11);
            if (grammarSymbols.Peek().IsRight)
                Error(grammarSymbols.Peek().GetRight.Location,
                    "Unexpected token: " + grammarSymbols.Peek().GetRight.Type);
            return new Just<IAtomNode>(grammarSymbols.Peek().GetLeft);
        }

        /***
         *   Precondition: grammarSymbols alternates between IAtomNodes, operator Tokens, .Count is odd
         *                 the precedences of the IAtomNodes is increasing.
         *   Postcondition: Either grammarSymbols.Count == 1, or everything in grammarSymbols will have precedence <= targetPrecedence.
         *
         */
        private void Reduce(Stack<IEither<IAtomNode, Token>> grammarSymbols, int targetPrecedence)
        {
            while (grammarSymbols.Count > 1) // && grammarSymbols.Peek().GetLeft.Precedence > targetPrecedence)
            {
                //These shouldn't error...
                var r = grammarSymbols.Pop().GetLeft;

                if (Precedences[grammarSymbols.Peek().GetRight.Type] > targetPrecedence)
                {
                    grammarSymbols.Push(new Left<IAtomNode, Token>(r));
                    break;
                }

                var op = grammarSymbols.Pop().GetRight;
                var l = grammarSymbols.Pop().GetLeft;

                grammarSymbols.Push(new Left<IAtomNode, Token>(new OperatorNode(l, op, r, l.Precedence)));
            }
        }

        private int GetLowestPrecedence(Stack<IEither<IAtomNode, Token>> grammarSymbols)
        {
            var minPrec = 11; //TODO: Note that this is the largest possible value.
            foreach (var e in grammarSymbols)
                e.Case(n => { minPrec = Math.Min(minPrec, n.Precedence); },
                    t => { minPrec = Math.Min(minPrec, Precedences[t.Type]); });
            return minPrec;
        }

        private IList<IAtomNode> ParseList(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes)
        {
            var head = tokens.Current;
            tokens.MoveNext();
            IList<IAtomNode> atoms = new List<IAtomNode>();
            while (tokens.Current.Type != TokenType.NEWLINE && tokens.Current.Type != TokenType.CLOSE_BRACKET)
            {
                var res = ParseAtom(tokens, scopes);
                res.IfJust(
                    n => atoms.Add(n),
                    () => Error(tokens.Current.Location, "Expected atomic value, got " + tokens.Current.Type + "."));
                if (tokens.Current.Type == TokenType.COMMA)
                    tokens.MoveNext();
            }

            if (tokens.Current.Type == TokenType.CLOSE_BRACKET)
                tokens.MoveNext();
            else
                Error(head.Location, "Unmatched open bracket.");
            return atoms;
        }

        public IMaybe<ILineNode> ParseLine(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes)
        {
            if (IsIncluding)
            {
                if (tokens.Current.Type == TokenType.NEWLINE || tokens.Current.Type == TokenType.SEMICOLON)
                {
                    tokens.MoveNext();
                    return new Nothing<ILineNode>();
                }

                _head = tokens.Current;
                switch (_head.Type)
                {
                    case TokenType.IDENTIFIER:
                    case TokenType.MAYBE_MACRO:
                        if (ExpandIdentifier(tokens, scopes)) return ParseLine(tokens, scopes);

                        tokens.MoveNext();
                        if (tokens.Current.Type == TokenType.COLON)
                        {
                            tokens.MoveNext();
                            if (scopes.Head.HasLocalLabel(_head.Content))
                                Warning(_head.Location,
                                    "Label already in scope, ignoring: " +
                                    _head.Content); //replacing: " + head.Content);
                            else if (!IsValidLabelName(_head.Content))
                                Error(_head.Location, "Invalid label name " + _head.Content + '.');
                            else
                                scopes.Head.AddLabel(_head.Content, CurrentOffset);

                            return new Nothing<ILineNode>();
                        }

                        tokens.PutBack(_head);
                        return ParseStatement(tokens, scopes).Fmap(n => (ILineNode)n);
                    case TokenType.OPEN_BRACE:
                        return new Just<ILineNode>(ParseBlock(tokens,
                            new ImmutableStack<Closure>(new Closure(), scopes)));
                    case TokenType.PREPROCESSOR_DIRECTIVE:
                        return ParsePreprocessor(tokens, scopes);
                    case TokenType.OPEN_BRACKET:
                        Error(_head.Location, "Unexpected list literal.");
                        IgnoreRestOfLine(tokens);
                        break;
                    case TokenType.NUMBER:
                    case TokenType.OPEN_PAREN:
                        Error(_head.Location, "Unexpected mathematical expression.");
                        IgnoreRestOfLine(tokens);
                        break;
                    default:
                        tokens.MoveNext();
                        Error(_head.Location, string.Format("Unexpected token: {0}: {1}", _head.Type, _head.Content));
                        IgnoreRestOfLine(tokens);
                        break;
                }

                return new Nothing<ILineNode>();
            }

            var hasNext = true;
            while (tokens.Current.Type != TokenType.PREPROCESSOR_DIRECTIVE && (hasNext = tokens.MoveNext())) ;
            if (hasNext)
            {
                return ParsePreprocessor(tokens, scopes);
            }

            Error(null, string.Format("Missing {0} endif(s).", Inclusion.Count));
            return new Nothing<ILineNode>();
        }

        private IMaybe<ILineNode> ParsePreprocessor(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes)
        {
            _head = tokens.Current;
            tokens.MoveNext();
            //Note: Not a ParseParamList because no commas.
            var paramList = ParsePreprocParamList(tokens, scopes);
            var retVal = _directiveHandler.HandleDirective(this, _head, paramList, tokens);
            if (!retVal.IsNothing)
            {
                CheckDataWrite(retVal.FromJust.Size);
                CurrentOffset += retVal.FromJust.Size;
            }

            return retVal;
        }

        /***
         *   Precondition: tokens.Current.Type == TokenType.IDENTIFIER || MAYBE_MACRO
         *   Postcondition: tokens.Current is fully reduced (i.e. not a macro, and not a definition)
         *   Returns: true iff tokens was actually expanded.
         */
        public bool ExpandIdentifier(MergeableGenerator<Token> tokens, ImmutableStack<Closure> scopes)
        {
            var ret = false;
            //Macros and Definitions.
            if (tokens.Current.Type == TokenType.MAYBE_MACRO && Macros.ContainsName(tokens.Current.Content))
            {
                var head = tokens.Current;
                tokens.MoveNext();
                var parameters = ParseMacroParamList(tokens);
                if (Macros.HasMacro(head.Content, parameters.Count))
                    tokens.PrependEnumerator(Macros.GetMacro(head.Content, parameters.Count)
                        .ApplyMacro(head, parameters, scopes).GetEnumerator());
                else
                    Error(head.Location,
                        string.Format("No overload of {0} with {1} parameters.", head.Content, parameters.Count));
                return true;
            }

            if (tokens.Current.Type == TokenType.MAYBE_MACRO)
            {
                var head = tokens.Current;
                tokens.MoveNext();
                tokens.PutBack(new Token(TokenType.IDENTIFIER, head.Location, head.Content));
                return true;
            }

            if (Definitions.ContainsKey(tokens.Current.Content))
            {
                var head = tokens.Current;
                tokens.MoveNext();
                tokens.PrependEnumerator(Definitions[head.Content].ApplyDefinition(head).GetEnumerator());
                return true;
            }

            return ret;
        }

        public void Message(Location? loc, string message)
        {
            log.Message(Log.MsgKind.MESSAGE, loc, message);
        }

        public void Warning(Location? loc, string message)
        {
            log.Message(Log.MsgKind.WARNING, loc, message);
        }

        public void Error(Location? loc, string message)
        {
            log.Message(Log.MsgKind.ERROR, loc, message);
        }

        private void IgnoreRestOfStatement(MergeableGenerator<Token> tokens)
        {
            while (tokens.Current.Type != TokenType.NEWLINE && tokens.Current.Type != TokenType.SEMICOLON &&
                   tokens.MoveNext()) ;
            if (tokens.Current.Type == TokenType.SEMICOLON) tokens.MoveNext();
        }

        private void IgnoreRestOfLine(MergeableGenerator<Token> tokens)
        {
            while (tokens.Current.Type != TokenType.NEWLINE && tokens.MoveNext()) ;
        }

        public void Clear()
        {
            Macros.Clear();
            Definitions.Clear();
            Raws.Clear();
            Inclusion = ImmutableStack<bool>.Nil;
            CurrentOffset = 0;
            _pastOffsets.Clear();
        }

        private string PrettyPrintParams(IList<IParamNode> parameters)
        {
            var sb = new StringBuilder();
            foreach (var parameter in parameters)
            {
                sb.Append(parameter.PrettyPrint());
                sb.Append(' ');
            }

            return sb.ToString();
        }

        // Return value: Location where protection occurred. Nothing if location was not protected.
        private IMaybe<Location> IsProtected(int offset, int length)
        {
            foreach (var protectedRegion in _protectedRegions)
                //They intersect if the last offset in the given region is after the start of this one
                //and the first offset in the given region is before the last of this one
                if (offset + length > protectedRegion.Item1 && offset < protectedRegion.Item1 + protectedRegion.Item2)
                    return new Just<Location>(protectedRegion.Item3);
            return new Nothing<Location>();
        }

        private void CheckDataWrite(int length)
        {
            // TODO: maybe make this warning optional?
            if (!_offsetInitialized)
            {
                Warning(_head.Location,
                    "Writing before initializing offset. You may be breaking the ROM! (use `ORG offset` to set write offset).");
                _offsetInitialized = false; // only warn once
            }

            // TODO (maybe?): save Location of PROTECT statement, for better diagnosis
            // We would then print something like "Trying to write data to area protected at <location>"

            var prot = IsProtected(CurrentOffset, length);
            if (!prot.IsNothing)
            {
                var l = prot.FromJust;
                Error(_head.Location,
                    string.Format("Trying to write data to area protected in file {0} at line {1}, column {2}.",
                        Path.GetFileName(l.file), l.lineNum, l.colNum));
            }
        }
    }
}
