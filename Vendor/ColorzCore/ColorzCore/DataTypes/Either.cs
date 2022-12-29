using System;

namespace ColorzCore.DataTypes
{
#pragma warning disable IDE1006 // Naming Styles
    public interface IEither<TLeft, TRight>
#pragma warning restore IDE1006 // Naming Styles
    {
        bool IsLeft { get; }
        bool IsRight { get; }
        TLeft GetLeft { get; }
        TRight GetRight { get; }
        void Case(Action<TLeft> lAction, Action<TRight> rAction);
    }

    public class Left<TL, TR> : IEither<TL, TR>
    {
        public Left(TL val)
        {
            GetLeft = val;
        }

        public bool IsLeft => true;
        public bool IsRight => false;
        public TL GetLeft { get; }
        public TR GetRight => throw new WrongEitherException();

        public void Case(Action<TL> lAction, Action<TR> rAction)
        {
            lAction(GetLeft);
        }
    }

    public class Right<TL, TR> : IEither<TL, TR>
    {
        public Right(TR val)
        {
            GetRight = val;
        }

        public bool IsLeft => false;
        public bool IsRight => true;
        public TL GetLeft => throw new WrongEitherException();
        public TR GetRight { get; }

        public void Case(Action<TL> lAction, Action<TR> rAction)
        {
            rAction(GetRight);
        }
    }

    internal class WrongEitherException : Exception
    {
    }
}
