// TokenExtensions.cs
using System;


// Extension methods for token invocation and registration
// Allows shorter invocation of tokens without needing to reference GameContext directly
// (Or implement as an API within GameContext)

public static class TokenExtensions
{
    public static void Invoke<TArg>(this Act<TArg> token, TArg arg)
        => GameContext.Instance.Act(token, arg);

    public static bool TryInvoke<TArg>(this Act<TArg> token, TArg arg)
        => GameContext.Instance.TryAct(token, arg);

    public static void Register<TArg>(this Act<TArg> token, Action<TArg> handler)
        => GameContext.Instance.AddAct(token, handler);

    public static T Ask<T>(this Ask<T> token)
        => GameContext.Instance.Ask(token);

    public static bool TryAsk<T>(this Ask<T> token, out T result)
        => GameContext.Instance.TryAsk(token, out result);

    public static void Register<T>(this Ask<T> token, Func<T> handler)
        => GameContext.Instance.AddAsk(token, handler);
}