using System;

namespace ColorzCore.DataTypes
{
    public delegate TR UnaryFunction<T, TR>(T val);

    public delegate TR RConst<TR>();

    public delegate void Action<T>(T val);

    public delegate void NullaryAction();

    public delegate IMaybe<TR> MaybeAction<T, TR>(T val);

#pragma warning disable IDE1006 // Naming Styles
    public interface IMaybe<T>
#pragma warning restore IDE1006 // Naming Styles
    {
        bool IsNothing { get; }
        T FromJust { get; }
        IMaybe<TR> Fmap<TR>(UnaryFunction<T, TR> f);
        IMaybe<TR> Bind<TR>(MaybeAction<T, TR> f);
        TR IfJust<TR>(UnaryFunction<T, TR> just, RConst<TR> nothing);
        void IfJust(Action<T> just, NullaryAction nothing = null);
    }

    public class Just<T> : IMaybe<T>
    {
        public Just(T val)
        {
            FromJust = val;
        }

        public bool IsNothing => false;
        public T FromJust { get; }

        public IMaybe<TR> Fmap<TR>(UnaryFunction<T, TR> f)
        {
            return new Just<TR>(f(FromJust));
        }

        public IMaybe<TR> Bind<TR>(MaybeAction<T, TR> f)
        {
            return f(FromJust);
        }

        public TR IfJust<TR>(UnaryFunction<T, TR> just, RConst<TR> nothing)
        {
            return just(FromJust);
        }

        public void IfJust(Action<T> just, NullaryAction nothing)
        {
            just(FromJust);
        }
    }

    public class Nothing<T> : IMaybe<T>
    {
        public bool IsNothing => true;
        public T FromJust => throw new MaybeException();

        public IMaybe<TR> Fmap<TR>(UnaryFunction<T, TR> f)
        {
            return new Nothing<TR>();
        }

        public IMaybe<TR> Bind<TR>(MaybeAction<T, TR> f)
        {
            return new Nothing<TR>();
        }

        public TR IfJust<TR>(UnaryFunction<T, TR> just, RConst<TR> nothing)
        {
            return nothing();
        }

        public void IfJust(Action<T> just, NullaryAction nothing)
        {
            nothing?.Invoke();
        }
    }

    public class MaybeException : Exception
    {
    }
}
