namespace Dungeon
{
    public interface ITrigger { }

    public interface ITrigger<TResult> : ITrigger
    {
        TResult Trigger();
    }

    public interface ITrigger<TResult, TArg> : ITrigger
    {
        TResult Trigger(TArg arg1);
    }

    public interface ITrigger<TResult, TArg, TArg2> : ITrigger
    {
        TResult Trigger(TArg arg1, TArg2 arg2);
    }

    public interface ITrigger<TResult, TArg, TArg2, TArg3> : ITrigger
    {
        TResult Trigger(TArg arg1, TArg2 arg2, TArg3 arg3);
    }

    public interface ITrigger<TResult, TArg, TArg2, TArg3, TArg4> : ITrigger
    {
        TResult Trigger(TArg arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);
    }
}