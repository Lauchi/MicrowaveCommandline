﻿using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat.Members.Methods
{
    internal class MethodSingleParamFinishedState : ParseState
    {
        public MethodSingleParamFinishedState(Parser parser) : base(parser)
        {
        }

        public override ParseState Parse(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.ParamSeparator:
                    return AdditionalParamStateFound();
                case TokenType.ParameterBracketClose:
                    return MethodParamClosedStateFound();
                default:
                    throw new NoTransitionException(token);
            }
        }

        private ParseState MethodParamClosedStateFound()
        {
            return new MethodParamEndedState(Parser);
        }

        private ParseState AdditionalParamStateFound()
        {
            return new AdditionalMethodParamState(Parser);
        }
    }
}